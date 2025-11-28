#!/bin/bash
# Test script til at tjekke SQL Server forbindelse

echo "Tester SQL Server forbindelse..."

# Prøv forskellige stier til sqlcmd
if docker exec coherence-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT @@VERSION" 2>/dev/null; then
    echo "✅ SQL Server er klar (mssql-tools18)"
    exit 0
elif docker exec coherence-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT @@VERSION" 2>/dev/null; then
    echo "✅ SQL Server er klar (mssql-tools)"
    exit 0
elif docker exec coherence-db /usr/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT @@VERSION" 2>/dev/null; then
    echo "✅ SQL Server er klar (usr/bin)"
    exit 0
else
    echo "❌ SQL Server er ikke klar eller sqlcmd ikke fundet"
    echo ""
    echo "Tjekker container status..."
    docker ps -a | grep coherence-db
    echo ""
    echo "Tjekker logs..."
    docker logs coherence-db --tail 20
    exit 1
fi

