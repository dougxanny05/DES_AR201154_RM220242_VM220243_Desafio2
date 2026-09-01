using Eventos.BL.Interfaces;
using Eventos.BL.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Eventos.BL.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceConnector(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<EventoProfile>();
                cfg.AddProfile<ParticipanteProfile>();
                cfg.AddProfile<OrganizadorProfile>();
            });

            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IParticipanteService, ParticipanteService>();
            services.AddTransient<IOrganizadorService, OrganizadorService>();
            return services;
        }
    }
}
