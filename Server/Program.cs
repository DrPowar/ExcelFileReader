
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Commands;
using Server.DB;
using Server.Handlers;
using Server.Repositories;
using Server.Services;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });

            builder.Services.AddSingleton<IAddPeopleService, AddPeopleService>();
            builder.Services.AddSingleton<IAddPersonService, AddPersonService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ExcelDBContext>(options =>
                options.UseSqlServer(connectionString));

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
        }
    }
}
