using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;
using ClubeBeneficios.Customers.Domain.Dtos.PartnerCustomerPortal;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Infrastructure.Context;
using Dapper;
using System.Data;

namespace ClubeBeneficios.Customers.Infrastructure.Repositories;

public sealed class PartnerCustomerPortalRepository : IPartnerCustomerPortalRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PartnerCustomerPortalRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid?> GetPartnerCustomerIdByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<Guid?>(
            new CommandDefinition(
                "SELECT TOP (1) id FROM dbo.partner_customers WHERE user_id = @UserId ORDER BY created_at DESC;",
                new { UserId = userId },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<PartnerCustomerBenefitDashboardDto?> GetBenefitDashboardAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(
                "dbo.usp_partner_customer_benefit_dashboard_summary",
                new { PartnerCustomerId = partnerCustomerId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        var summary = await multi.ReadFirstOrDefaultAsync<PartnerCustomerBenefitDashboardSummaryDto>();
        var recentRequests = (await multi.ReadAsync<BenefitRequestListItemDto>()).ToList();

        if (summary is null)
        {
            return null;
        }

        return new PartnerCustomerBenefitDashboardDto
        {
            Summary = summary,
            RecentRequests = recentRequests
        };
    }

    public async Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetBenefitRequestsAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var result = await connection.QueryAsync<BenefitRequestListItemDto>(
            new CommandDefinition(
                "dbo.usp_partner_customer_benefit_requests_list",
                new { PartnerCustomerId = partnerCustomerId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.ToList();
    }
}