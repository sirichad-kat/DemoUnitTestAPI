using DemoUnitTestLib.DL;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace DemoUnitTestLib.BL
{

    public interface IFwService
    {
        DTO.FwInit FindFwInit(LabCommonLib.Model.LookupDataRequest dat);
        LabCommonLib.Model.ResponseData<DTO.FwInit> SaveFwInit(Model.MFwInit dat);
    }

    public class FwService : IFwService
    {
        IConfiguration configuration;
        private IFwRepo repo;
        public FwService(IConfiguration _configuration, IFwRepo _repo)
        {
            configuration = _configuration;
            repo = _repo;
        }

        public DTO.FwInit FindFwInit(LabCommonLib.Model.LookupDataRequest dat)
        {
            DTO.FwInit ret = null;
            if (dat.Connection != null && dat.Connection != "")
            {
                try
                {
                    ret = repo.FindFwInitInternal(dat.Connection, dat.TextSearch);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException("Connection is empty");
            }
            return ret;
        }


        public LabCommonLib.Model.ResponseData<DTO.FwInit> SaveFwInit(Model.MFwInit dat)
        {
            LabCommonLib.Model.ResponseData<DTO.FwInit> ret = new LabCommonLib.Model.ResponseData<DTO.FwInit>();
            try
            {
                DTO.FwInit saveModel = new DTO.FwInit();
                saveModel.RetrieveFromDTO(dat);
                saveModel.Accessdate = DateTime.Now;

                if (dat.IsNew)
                {
                    //check isExist
                    if (repo.IsFwInitExist(dat.Connection, dat.KeyName))
                    {
                        repo.InsertFwInit(dat.Connection, saveModel);
                        ret.IsValid = true;
                    }
                    else
                    {
                        ret.IsValid = false;
                        ret.Message = "Cannot save duplicate record";
                    }
                }
                else
                {
                    repo.UpdateFwInit(dat.Connection, saveModel);
                    ret.IsValid = true;
                }
                if (ret.IsValid)
                {
                    ret.Data = FindFwInit(new LabCommonLib.Model.LookupDataRequest() { Connection = dat.Connection, TextSearch = dat.KeyName });
                }

            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }
    }
}