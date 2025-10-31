# Christians side — Architecture & Model Overview
_Last updated: 2025-10-03 11:03._

This document captures the **current agreed design** for Christians side. It follows a **hybrid architecture**: Clean Architecture in the core (Domain, Application, Infrastructure) and **Vertical Slice** structure in the **Web** layer. It also reflects the **slim content model** (Post + MediaAttachment) and **Reviews (Solution A)**.

---

## Layers & Projects
- **ChristiansSide.Domain** — Pure domain types (entities, value objects, rules).
- **ChristiansSide.Application** — Use cases, DTOs, and **interfaces** (repositories, gateways, notifiers).
- **ChristiansSide.Infrastructure** — EF Core DbContext + repositories, email provider, and AI gateway implementations.
- **ChristiansSide.Web** (Blazor Server) — UI (Owner dashboard + User app) with **feature-by-feature** folders (vertical slices).

### Web (Vertical Slice by feature)
```
/Features
  /Posts          # Post + MediaAttachment (Blog/HealthTip, paywall)
    /Create /List /Details /Publish /Edit /Delete
  /Reviews
    /Owner        # TextReview / ImageReview / VideoReview (ReviewBase + Category=Book|Product)
      /CreateText /CreateImage /CreateVideo /List /Approve /Delete
    /User         # small user reviews with approval
      /Submit /ListMine /Approve /Reject
  /Profiles       # UserProfile (Age, Gender, HealthNote)
    /Upsert /Get
  /Membership     # Paywall access
    /Activate /Deactivate /Get
  /Chat           # UI + calls to Python (no persistence)
    /Ask
/Areas
  /OwnerDashboard  # pages/components for owner
  /UserApp         # pages/components for users
/Auth              # policies/role requirements
Program.cs
```

---

## Domain Model (Key Entities)
- **User** (Identity): `Id, Email, PasswordHash, Role(Owner|User), CreatedAt`
- **UserProfile**: `UserId, Age, Gender, HealthNote`
- **Membership**: `UserId, IsActive, StartedAt, ExpiresAt?`
- **Post**: `AuthorId(Owner), Title, Body(Markdown), Category(Blog|HealthTip), IsPaywalled, IsPublished, CreatedAt`
- **MediaAttachment**: `PostId, MediaType(Image|Video), Url, AltText?, Caption?, Duration?`
- **ReviewBase (abstract)**: `AuthorId(Owner), Title, CreatedAt, IsApproved, Category(Book|Product)`  
  - **TextReview**: `Content`  
  - **ImageReview**: `Body?` + **ReviewImage**: `ImageReviewId, Url, AltText?`  
  - **VideoReview** + **ReviewVideo**: `VideoReviewId, Url, Caption?, Duration?`
- **UserReview** (user feedback): `UserId, Title, Content, CreatedAt, Status(Pending|Approved|Rejected)`

### Text ERD (spec-style)
```
USER {
  GUID Id PK
  STRING Email UK
  STRING PasswordHash
  STRING Role   // Owner | User
  DATETIME CreatedAt
}

USERPROFILE {
  GUID Id PK
  GUID UserId FK  // -> USER
  INT Age
  STRING Gender   // Male | Female | Other | Unspecified
  TEXT HealthNote
}

MEMBERSHIP {
  GUID Id PK
  GUID UserId FK  // -> USER
  BOOL IsActive
  DATETIME StartedAt
  DATETIME? ExpiresAt
}

POST {
  GUID Id PK
  GUID AuthorId FK   // -> USER (Owner)
  STRING Title
  TEXT Body          // Markdown/HTML
  STRING Category    // Blog | HealthTip
  BOOL IsPaywalled   // true for HealthTip
  BOOL IsPublished
  DATETIME CreatedAt
}

MEDIAATTACHMENT {
  GUID Id PK
  GUID PostId FK     // -> POST
  STRING MediaType   // Image | Video
  STRING Url
  STRING? AltText
  STRING? Caption
  TIME? Duration     // for videos (or ISO 8601)
}

REVIEWBASE {
  GUID Id PK
  GUID AuthorId FK   // -> USER (Owner)
  STRING Title
  DATETIME CreatedAt
  BOOL IsApproved
  STRING Category    // Book | Product
}

TEXTREVIEW {
  GUID Id PK FK      // -> REVIEWBASE (1:1 inheritance)
  TEXT Content
}

IMAGEREVIEW {
  GUID Id PK FK      // -> REVIEWBASE (1:1 inheritance)
  STRING? Body       // optional text/body for the image review
}

REVIEWIMAGE {
  GUID Id PK
  GUID ImageReviewId FK  // -> IMAGEREVIEW
  STRING Url
  STRING? AltText
}

VIDEOREVIEW {
  GUID Id PK FK      // -> REVIEWBASE (1:1 inheritance)
}

REVIEWVIDEO {
  GUID Id PK
  GUID VideoReviewId FK  // -> VIDEOREVIEW
  STRING Url
  STRING? Caption
  TIME? Duration
}

USERREVIEW {
  GUID Id PK
  GUID UserId FK     // -> USER (author)
  STRING Title
  TEXT Content
  DATETIME CreatedAt
  STRING Status      // Pending | Approved | Rejected
}
```

---

## Application Layer (Interfaces & Use Cases — excerpt)
- **Content**
  - `IPostRepository`: `Add`, `GetById`, `List(PostQuery)`, `Update`, `Delete`
- **OwnerReviews**
  - `IReviewRepository`: `Add(ReviewBase)`, `Approve(id)`, `List(ReviewQuery)`
- **UserReviews**
  - `IUserReviewRepository`: `Add(Pending)`, `SetStatus(id, status)`, `List(UserReviewQuery)`  
  - `IEmailNotifier`: `SendReviewModerationAsync(reviewId, approveLink, rejectLink)`
- **Users / Profiles / Membership**
  - `IUserProfileRepository`: `Upsert`, `GetByUserId`
  - `IMembershipRepository`: `GetByUserId`, `Activate`, `Deactivate`
- **AI**
  - `IAIChatGateway`: `SendAsync(userId, prompt, context)` → Python service

> **Guideline:** no `IQueryable` escaping repositories; return DTOs/lists or use specifications.

---

## Infrastructure (Implementations)
- **Persistence**
  - `AppDbContext` + EF mappings (Post, MediaAttachment, Reviews, UserProfile, Membership, UserReview)
  - Repos: `PostRepository`, `ReviewRepository`, `UserReviewRepository`, `UserProfileRepository`, `MembershipRepository`
- **Gateways**
  - `AiChatGatewayHttp` → Python
  - `EmailNotifierSmtp` (or provider)

---

## Security & Access
- **Roles:** `Owner`, `User`
- Owner Dashboard requires `Owner`.
- HealthTip content requires `User` + `Membership.IsActive == true`.
- Chat requires `User`-login (no DB persistence of messages).

---

## Routing (Examples)
- `GET /posts?category=Blog|HealthTip`
- `POST /posts` (Owner)
- `POST /reviews/owner/text|image|video` (Owner)
- `POST /reviews/user/submit` (User) → email to Owner → `POST /reviews/user/<built-in function id>/approve`
- `POST /membership/activate|deactivate` (Owner/admin)
- `POST /chat/ask` (User → Python AI)

---

## Diagram (ERD PNG)
The current ERD image is exported here:
- **ERD (combined):** [erd_christians_side_combined.png](sandbox:/mnt/data/erd_christians_side_combined.png)
