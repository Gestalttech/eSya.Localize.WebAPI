using eSya.Localize.DL.Entities;
using eSya.Localize.DO;
using eSya.Localize.IF;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Localize.DL.Repository
{
    public class CultureRepository : ICultureRepository
    {
        private readonly IStringLocalizer<CultureRepository> _localizer;
        public CultureRepository(IStringLocalizer<CultureRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Language Culture
        public async Task<List<DO_LanguageController>> GetResources()
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    return await db.GtEcltfcs.Where(x => x.ActiveStatus == true).Select
                    (r => new DO_LanguageController
                    {
                        ResourceName = r.ResourceName
                    }).Distinct().ToListAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_LanguageCulture>> GetLanguageCulture(string Culture, string Resource)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    if (Resource == "All")
                    {
                        //return await db.GtEcltfcs
                        //   .GroupJoin(db.GtEcltcds.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                        //   a => a.ResourceId,
                        //   f => f.ResourceId,
                        //   (a, f) => new { a, f = f.FirstOrDefault() })
                        //   .Select(r => new DO_LanguageCulture
                        //   {
                        //       ResourceId = r.a.ResourceId,
                        //       ResourceName = r.a.ResourceName,
                        //       Key = r.a.Key,
                        //       Value = r.a.Value,
                        //       Culture = r.f != null ? r.f.Culture : "",
                        //       CultureValue = r.f != null ? r.f.Value : ""

                        //   }).ToListAsync();

                        var result= db.GtEcltfcs
                     .GroupJoin(db.GtEcltcds.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                      a => a.ResourceId,
                      f => f.ResourceId,
                     (resu, cul) => new { resu, cul })
                    .SelectMany(z => z.cul.DefaultIfEmpty(),
                     (a, b) => new DO_LanguageCulture
                     {
                         ResourceId = a.resu.ResourceId,
                         ResourceName = a.resu.ResourceName,
                         Key = a.resu.Key,
                         Value = a.resu.Value,
                         Culture = b == null ? "" : b.Culture,
                         CultureValue = b == null ? "" : b.Value,
                     }).ToList();
                        var DistinctKeys = result.GroupBy(x => x.Key).Select(y => y.First());
                        return DistinctKeys.ToList();

                    }
                    else
                    {
                        //return await db.GtEcltfcs.Where(x => x.ResourceName.ToUpper().Trim() == Resource.ToUpper().Trim())
                        //  .GroupJoin(db.GtEcltcds.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                        //  a => a.ResourceId,
                        //  f => f.ResourceId,
                        //  (a, f) => new { a, f = f.FirstOrDefault() })
                        //  .Select(r => new DO_LanguageCulture
                        //  {
                        //      ResourceId = r.a.ResourceId,
                        //      ResourceName = r.a.ResourceName,
                        //      Key = r.a.Key,
                        //      Value = r.a.Value,
                        //      Culture = r.f != null ? r.f.Culture : "",
                        //      CultureValue = r.f != null ? r.f.Value : ""

                        //  }).ToListAsync();

                        var result = db.GtEcltfcs.Where(x => x.ResourceName.ToUpper().Trim() == Resource.ToUpper().Trim())
                 .GroupJoin(db.GtEcltcds.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                  a => a.ResourceId,
                  f => f.ResourceId,
                 (resu, cul) => new { resu, cul })
                .SelectMany(z => z.cul.DefaultIfEmpty(),
                 (a, b) => new DO_LanguageCulture
                 {
                     ResourceId = a.resu.ResourceId,
                     ResourceName = a.resu.ResourceName,
                     Key = a.resu.Key,
                     Value = a.resu.Value,
                     Culture = b == null ? "" : b.Culture,
                     CultureValue = b == null ? "" : b.Value,
                 }).ToList();

                        var DistinctKeys = result.GroupBy(x => x.Key).Select(y => y.First());
                        return DistinctKeys.ToList();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLanguageCulture(List<DO_LanguageCulture> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var rc in obj.Where(x => x.CultureValue != null && x.CultureValue != ""))
                        {
                            GtEcltcd r_culture = db.GtEcltcds.Where(x => x.ResourceId == rc.ResourceId
                                            && x.Culture.ToUpper().Trim() == rc.Culture.ToUpper().Trim()).FirstOrDefault();
                            if (r_culture == null)
                            {
                                var add = new GtEcltcd
                                {
                                    ResourceId = rc.ResourceId,
                                    Culture = rc.Culture,
                                    Value = rc.CultureValue,
                                    ActiveStatus = true,
                                    FormId = rc.FormId,
                                    CreatedBy = rc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = rc.TerminalID
                                };
                                db.GtEcltcds.Add(add);
                            }
                            else
                            {
                                r_culture.Value = rc.CultureValue;
                                r_culture.ActiveStatus = true;
                                r_culture.ModifiedBy = rc.UserID;
                                r_culture.ModifiedOn = System.DateTime.Now;
                                r_culture.ModifiedTerminal = rc.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

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
        #endregion 

        #region Culture Keys
        public List<DO_LanguageCulture> GetDistinictCultureKeys(string Culture)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    //var result = db.GtEcltfcs
                    //            .GroupJoin(db.GtEcltcds.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                    //            a => a.ResourceId,
                    //            f => f.ResourceId,
                    //            (a, f) => new { a, f = f.FirstOrDefault() })
                    //            .Select(r => new DO_LanguageCulture
                    //            {
                    //                Key = r.a.Key,
                    //                Value = r.a.Value,
                    //                CultureValue = r.f != null ? r.f.Value : ""

                    //            }).ToList();
                    //var DistinctKeys = result.GroupBy(x => x.Key).Select(y => y.First());
                    //return DistinctKeys.ToList();

                    var result = db.GtEcltfcs
                    .GroupJoin(db.GtEcltcds.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                      a => a.ResourceId,
                      f => f.ResourceId,
                     (resu, cul) => new { resu, cul })
                    .SelectMany(z => z.cul.DefaultIfEmpty(),
                     (a, b) => new DO_LanguageCulture
                     {
                         Key = a.resu.Key,
                         Value = a.resu.Value,
                         CultureValue = b == null ? "" : b.Value
                     }).ToList();
                    var DistinctKeys = result.GroupBy(x => x.Key).Select(y => y.First());
                    return DistinctKeys.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateCultureKeys(List<DO_LanguageCulture> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var rc in obj.Where(x => x.CultureValue != null && x.CultureValue != ""))
                        {
                            var ResourceIds = db.GtEcltfcs.Where(k => k.Key.ToUpper().Trim() == rc.Key.ToUpper().Trim()).ToList();

                            foreach (var resId in ResourceIds)
                            {
                                GtEcltcd r_culture = db.GtEcltcds.Where(x => x.ResourceId == resId.ResourceId
                                                && x.Culture.ToUpper().Trim() == rc.Culture.ToUpper().Trim()).FirstOrDefault();
                                if (r_culture == null)
                                {
                                    var add = new GtEcltcd
                                    {
                                        ResourceId = resId.ResourceId,
                                        Culture = rc.Culture,
                                        Value = rc.CultureValue,
                                        ActiveStatus = true,
                                        FormId = rc.FormId,
                                        CreatedBy = rc.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = rc.TerminalID
                                    };
                                    db.GtEcltcds.Add(add);
                                }
                                else
                                {
                                    r_culture.Value = rc.CultureValue;
                                    r_culture.ActiveStatus = true;
                                    r_culture.ModifiedBy = rc.UserID;
                                    r_culture.ModifiedOn = System.DateTime.Now;
                                    r_culture.ModifiedTerminal = rc.TerminalID;
                                }
                                await db.SaveChangesAsync();
                            }
                        }

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

        #endregion Culture
    }
}
