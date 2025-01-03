using Mart_SiparisTakipMVC.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mart_SiparisTakipMVC.Models
{
    public class MultipleModels
    {
        public IEnumerable<PagedList.IPagedList<GuncelSiparisler>> guncelSiparislers {  get; set; }
        public IEnumerable<GuncelSiparisler> GuncelSiparislersModel { get;}
    }
}