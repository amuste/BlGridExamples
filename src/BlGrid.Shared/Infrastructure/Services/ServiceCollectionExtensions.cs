using BlGrid.Infrastructure.Services;
using DnetAdminDashboard.Infrastructure.Services;
using DnetOverlayComponent.Infrastructure.Services;
using DnetSpinnerComponent.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlGrid.Shared.Infrastructure.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlGridComponent(this IServiceCollection services)
        {
            services.AddDnetOverlay();

            services.AddBlGrid(options =>
            {
                options.UseLicense("S*a!d*a*nI!SWzP@Pm*C!J@O*q*FU!E*v*Ca*q!J*B!J#M!T#S*G!J#B@vZ#D@k*vG!P!c*z!p@A*" +
                                   "C*h*z!NM*D@B@Jq*AJk!GV#C#a#gXG*Q#I#t#Y!sr@Puq@w*TC*EW!caE@p*J@H*w@rb@PE#L@JW*H*T#Y" +
                                   "qE*z@j#V@b@p!f#q#e@M!T!b#z*b#N!Q@k!wh!D#X@Z@P*g@P*Oe!d!a*n#j!sB*Z!");
            });

            services.AddDnetAdminDashboard();

            services.AddSpinner();

            return services;
        }
    }
}
