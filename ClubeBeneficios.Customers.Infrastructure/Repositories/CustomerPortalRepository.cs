using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Infrastructure.Context;
using Dapper;
using System.Data;

namespace ClubeBeneficios.Customers.Infrastructure.Repositories;

public sealed class CustomerPortalRepository : ICustomerPortalRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CustomerPortalRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid?> GetClientIdByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<Guid?>(
            new CommandDefinition(
                "SELECT TOP (1) id FROM dbo.clients WHERE user_id = @UserId ORDER BY created_at DESC;",
                new { UserId = userId },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<CustomerBenefitDashboardDto?> GetBenefitDashboardAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(
                "dbo.usp_client_benefit_dashboard_summary",
                new { ClientId = clientId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        var summary = await multi.ReadFirstOrDefaultAsync<CustomerBenefitDashboardSummaryDto>();
        var recentRequests = (await multi.ReadAsync<BenefitRequestListItemDto>()).ToList();

        if (summary is null)
        {
            return null;
        }

        return new CustomerBenefitDashboardDto
        {
            Summary = summary,
            RecentRequests = recentRequests
        };
    }

    public async Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetBenefitRequestsAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var result = await connection.QueryAsync<BenefitRequestListItemDto>(
            new CommandDefinition(
                "dbo.usp_client_benefit_requests_list",
                new { ClientId = clientId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.ToList();
    }
}