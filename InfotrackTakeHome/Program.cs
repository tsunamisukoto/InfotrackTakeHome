using TakeHomeAssignmentServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Note in reality you would register this in modules or some equivalent (I come from an autofac background and we'd have modules and submodules that all got registered. This SHOULD show DI usage though :P
builder.Services.AddScoped<ISettlementBookingService, SettlementBookingService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
