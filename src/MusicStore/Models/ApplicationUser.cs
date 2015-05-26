using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace MusicStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public List<App> Apps { get; set; }
        public string AppsJson
        {
            get { return JsonConvert.SerializeObject(Apps); }
            set { Apps = JsonConvert.DeserializeObject<List<App>>(value); }
        }

        public ApplicationUser()
        {
            Apps = new List<App>();
        }
    }
}