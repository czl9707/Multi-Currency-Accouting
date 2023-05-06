namespace Accountant.Models;

using Accountant.Services.DB;

public class PaymentMethodCommon
{
    private IDBConnectionFactory DBConnectionFactory;
    private IDapperWrapperService DapperWrapperService;

    public PaymentMethodCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService)
    {
        this.DBConnectionFactory = dBConnectionFactory;
        this.DapperWrapperService = dapperWrapperService;

        this.InitMethodAsync().GetAwaiter().GetResult();
    }

    private string sqlSelect 
    {
        get => "Select method_id, method_name FROM tbl_pay_method;";
    }

    private string sqlUpdate {
        get => @"
            UPDATE tbl_pay_method
            SET method_name = @vmethod_name
            WHERE method_id = @vmethod_id
        ";
    }

    private string sqlInsert {
        get => @"
            INSERT INTO tbl_pay_method (method_name)
            VALUES (@vmethod_name)
        ";
    }

    private string sqlDelete {
        get => @"
            DELETE FROM tbl_pay_method
            WHERE method_id = @vmethod_id
        ";
    }

    private IEnumerable<PaymentMethod> Methods;

    public async Task<List<PaymentMethod>> GetAllMethodsAsync() => 
        this.Methods.ToList();

    public async Task<Dictionary<long, PaymentMethod>> GetAllMethodsAsDictAsync() => 
        this.Methods.ToDictionary(m => m.MethodId, m => m);

    public async Task UpdateMethodAsync(PaymentMethod method)
    {
        using var connection = this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.sqlUpdate,
            param: new {
                vmethod_id = method.MethodId,
                vmethod_name = method.MethodName
            }
        ).ConfigureAwait(false);

        await this.InitMethodAsync();
    }

    public async Task AddNewMethodAsync(PaymentMethod method)
    {
        using var connection = this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.sqlInsert,
            param: new {vmethod_name = method.MethodName}
        ).ConfigureAwait(false);

        await this.InitMethodAsync();
    }

    public async Task DeleteMethodAsync(long methodId)
    {
        using var connection = this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.sqlDelete,
            param: new {vmethod_id = methodId}
        ).ConfigureAwait(false);

        await this.InitMethodAsync();
    }

    private async Task InitMethodAsync(){
        using var connection = this.DBConnectionFactory.GetConnection();
        this.Methods = await this.DapperWrapperService.QueryAsync<PaymentMethod>(
            connection: connection, 
            sql: this.sqlSelect
        ).ConfigureAwait(false);
    }
}