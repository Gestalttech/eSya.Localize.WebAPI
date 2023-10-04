using eSya.Localize.DL.Entities;
using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace eSya.Localize.DL.Repository
{
    public class ControllerRepository : IControllerRepository
    {
        private readonly IStringLocalizer<ControllerRepository> _localizer;
        public ControllerRepository(IStringLocalizer<ControllerRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Language Controller
        public async Task<List<string>> GetAllControllers()
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    //return await db.GtEcfmfd.Where(x => x.ActiveStatus == true)
                    //   // .OrderBy(t=> t.ControllerName.Substring((t.ControllerName.IndexOf('/')) + 1))
                    //    .Select(t => t.ControllerName.Substring((t.ControllerName.IndexOf('/')) + 1).Replace("Index","Account"))
                    //    .Distinct().OrderBy(c => c).ToListAsync();

                    return await db.GtEbecnts.Where(x => x.ActiveStatus == true)
                            .Select(t => t.Controller.Replace("Index", "Account"))
                            .Distinct().OrderBy(c => c).ToListAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_LanguageController>> GetLanguageControllersbyResource(string Resource)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    if (Resource != "All")
                    {
                        return await db.GtEcltfcs.Where(x => x.ResourceName.ToUpper().Replace(" ", "") == Resource.ToUpper().Replace(" ", ""))
                          .Select(lc => new DO_LanguageController
                          {
                              ResourceId = lc.ResourceId,
                              ResourceName = lc.ResourceName,
                              Key = lc.Key,
                              Value = lc.Value,
                              ActiveStatus = lc.ActiveStatus,
                              FormId = lc.FormId
                          }).ToListAsync();
                    }
                    else
                    {
                        return await db.GtEcltfcs
                        .Select(lc => new DO_LanguageController
                        {
                            ResourceId = lc.ResourceId,
                            ResourceName = lc.ResourceName,
                            Key = lc.Key,
                            Value = lc.Value,
                            ActiveStatus = lc.ActiveStatus,
                            FormId = lc.FormId
                        }).ToListAsync();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLanguageController(DO_LanguageController lobj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcltfc lc = db.GtEcltfcs.Where(x => x.ResourceId == lobj.ResourceId).FirstOrDefault();

                        if (lc == null)
                        {
                            GtEcltfc is_KeyExists = db.GtEcltfcs.FirstOrDefault(k => k.Key.ToUpper().Replace(" ", "") == lobj.Key.ToUpper().Replace(" ", "") && k.ResourceName.ToUpper().Replace(" ", "") == lobj.ResourceName.ToUpper().Replace(" ", ""));
                            if (is_KeyExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0105", Message = string.Format(_localizer[name: "W0105"]) };
                            }
                            int maxval = db.GtEcltfcs.Select(c => c.ResourceId).DefaultIfEmpty().Max();
                            int ResourceId = maxval + 1;
                            lc = new GtEcltfc
                            {
                                ResourceId = ResourceId,
                                ResourceName = lobj.ResourceName.Trim(),
                                Key = lobj.Key.Trim(),
                                Value = lobj.Value.Trim(),
                                ActiveStatus = lobj.ActiveStatus,
                                FormId = lobj.FormId,
                                CreatedBy = lobj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = lobj.TerminalID
                            };
                            db.GtEcltfcs.Add(lc);
                        }
                        else
                        {
                            GtEcltfc is_KeyExists = db.GtEcltfcs.FirstOrDefault(k => k.Key.ToUpper().Replace(" ", "") == lobj.Key.ToUpper().Replace(" ", "") && k.ResourceName.ToUpper().Replace(" ", "") == lobj.ResourceName.ToUpper().Replace(" ", "")
                            && k.ResourceId != lobj.ResourceId);
                            if (is_KeyExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0105", Message = string.Format(_localizer[name: "W0105"]) };

                            }
                            lc.ResourceName = lobj.ResourceName.Trim();
                            lc.Key = lobj.Key.Trim();
                            lc.Value = lobj.Value.Trim();
                            lc.ActiveStatus = lobj.ActiveStatus;
                            lc.ModifiedBy = lobj.UserID;
                            lc.ModifiedOn = System.DateTime.Now;
                            lc.ModifiedTerminal = lobj.TerminalID;
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }

                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }

                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveLanguageController(bool status, int ResourceId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcltfc lan_controller = db.GtEcltfcs.Where(w => w.ResourceId == ResourceId).FirstOrDefault();
                        if (lan_controller == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0106", Message = string.Format(_localizer[name: "W0106"]) };
                        }

                        lan_controller.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
}
