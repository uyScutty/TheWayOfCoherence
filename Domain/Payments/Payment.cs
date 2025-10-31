using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Payments.Enums;

namespace Domain.Payments
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = "BTC";
        public string TransactionId { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Payment(Guid userId, decimal amount)
        {
            UserId = userId;
            Amount = amount;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted(string txId)
        {
            TransactionId = txId;
            Status = PaymentStatus.Completed;
        }

        public void MarkAsFailed() => Status = PaymentStatus.Failed;
    }

}
