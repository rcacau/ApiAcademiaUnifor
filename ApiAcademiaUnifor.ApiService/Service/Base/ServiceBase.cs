using Supabase;

namespace ApiAcademiaUnifor.ApiService.Service.Base
{
    public class ServiceBase
    {
        protected readonly Client _supabase;

        public ServiceBase(Client supabase)
        {
            _supabase = supabase;
        }
    }
}
