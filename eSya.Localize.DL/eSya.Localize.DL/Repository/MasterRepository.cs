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
   public class MasterRepository: IMasterRepository
    {
        private readonly IStringLocalizer<MasterRepository> _localizer;
        public MasterRepository(IStringLocalizer<MasterRepository> localizer)
        {
            _localizer = localizer;
        }


        #region Localization Table Mapping

        public async Task<List<DO_LocalizationMaster>> GetLocalizationTableMaster()
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    return await db.GtEclttms
                    .Select(t => new DO_LocalizationMaster
                    {
                        TableCode = t.TableCode,
                        TableName = t.TableName,
                        SchemaName = t.SchemaName,
                        ActiveStatus = t.ActiveStatus
                    }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLocalizationTableMaster(DO_LocalizationMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.IsInsert)
                        {
                            if (db.GtEclttms.Where(t => t.TableCode == obj.TableCode).Count() > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0107", Message = string.Format(_localizer[name: "W0107"]) };
                            }
                        }

                        if (db.GtEclttms.Where(t => t.TableName.Trim().ToUpper() == obj.TableName.Trim().ToUpper() && t.TableCode != obj.TableCode).Count() > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0108", Message = string.Format(_localizer[name: "W0108"]) };
                        }
                        else if (db.GtEclttms.Where(t => t.SchemaName.Trim().ToUpper() == obj.SchemaName.Trim().ToUpper() && t.TableCode != obj.TableCode).Count() > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0109", Message = string.Format(_localizer[name: "W0109"]) };
                        }
                        else
                        {
                            GtEclttm lm = db.GtEclttms.Where(x => x.TableCode == obj.TableCode).FirstOrDefault();

                            if (lm == null)
                            {
                                lm = new GtEclttm
                                {
                                    TableCode = obj.TableCode,
                                    SchemaName = obj.SchemaName,
                                    TableName = obj.TableName,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEclttms.Add(lm);
                            }
                            else
                            {
                                lm.SchemaName = obj.SchemaName;
                                lm.TableName = obj.TableName;
                                lm.ActiveStatus = obj.ActiveStatus;
                                lm.ModifiedBy = obj.UserID;
                                lm.ModifiedOn = System.DateTime.Now;
                                lm.ModifiedTerminal = obj.TerminalID;
                            }

                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveLocalizationTableMaster(bool status, int Tablecode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEclttm table_master = db.GtEclttms.Where(w => w.TableCode == Tablecode).FirstOrDefault();
                        if (table_master == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0110", Message = string.Format(_localizer[name: "W0110"]) };
                        }

                        table_master.ActiveStatus = status;
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

        #region Language Mapping
        public async Task<List<DO_LocalizationMaster>> GetLocalizationMaster()
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    return await db.GtEclttms.Where(x => x.ActiveStatus == true)
                         .Select(t => new DO_LocalizationMaster
                         {
                             TableCode = t.TableCode,
                             TableName = t.TableName,
                         }).ToListAsync();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLocalizationLanguageMapping(List<DO_LocalizationLanguageMapping> obj)
        {

            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var l in obj.Where(w => w.FieldDescLanguage != null && w.FieldDescLanguage != ""))
                        {
                            GtEclttl tm = db.GtEclttls.Where(x => x.LanguageCode == l.LanguageCode
                                            && x.TableCode == l.TableCode
                                            && x.TablePrimaryKeyId == l.TablePrimaryKeyId).FirstOrDefault();
                            if (tm == null)
                            {
                                var add = new GtEclttl
                                {
                                    LanguageCode = l.LanguageCode,
                                    TableCode = l.TableCode,
                                    TablePrimaryKeyId = l.TablePrimaryKeyId,
                                    FieldDescLanguage = l.FieldDescLanguage,
                                    ActiveStatus = true,
                                    FormId = l.FormId,
                                    CreatedBy = l.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = l.TerminalID
                                };
                                db.GtEclttls.Add(add);
                            }
                            else
                            {
                                tm.FieldDescLanguage = l.FieldDescLanguage;
                                tm.ActiveStatus = true;
                                tm.ModifiedBy = l.UserID;
                                tm.ModifiedOn = System.DateTime.Now;
                                tm.ModifiedTerminal = l.TerminalID;
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

        public List<DO_LocalizationLanguageMapping> GetLocalizationLanguageMapping(string languageCode, int tableCode)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    var SchemaName = db.GtEclttms.Where(w => w.TableCode == tableCode).FirstOrDefault().SchemaName;
                    eSyaEnterprise dbcon = new eSyaEnterprise();
                    var tableMasterDetail = GetTableKeyValue(dbcon, SchemaName);

                    if (tableMasterDetail != null)
                    {
                        var lm = tableMasterDetail
                            .GroupJoin(db.GtEclttls.Where(w => w.LanguageCode == languageCode
                                    && w.TableCode == tableCode),
                                m => m.TablePrimaryKeyId,
                                l => l.TablePrimaryKeyId,
                                (m, l) => new { m, l = l.FirstOrDefault() }).DefaultIfEmpty()
                                .Select(r => new DO_LocalizationLanguageMapping
                                {
                                    TablePrimaryKeyId = r.m.TablePrimaryKeyId,
                                    FieldDescription = r.m.FieldDesc,
                                    FieldDescLanguage = r.l != null ? r.l.FieldDescLanguage : "",
                                }).ToList();

                        return lm;
                    }
                    else
                        return null;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IEnumerable<DO_TableField> GetTableKeyValue(eSyaEnterprise db, string SchemaName)
        {
            if (SchemaName.ToUpper() == "GT_ECAPCT")
            {
                return db.GtEcapcts.Where(w => w.ActiveStatus)
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.CodeType,
                        FieldDesc = r.CodeTyepDesc
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECAPCD")
            {
                return db.GtEcapcds.Where(w => w.ActiveStatus)
                    .Join(db.GtEcapcts,
                    a => a.CodeType,
                    c => c.CodeType,
                    (a, c) => new { a, c })
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.a.ApplicationCode,
                        FieldDesc = r.c.CodeTyepDesc + " - " + r.a.CodeDesc
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECFMFD")
            {
                return db.GtEcfmfds
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.FormId,
                        FieldDesc = r.FormName
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECFMAC")
            {
                return db.GtEcfmacs
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.ActionId,
                        FieldDesc = r.ActionDesc
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECMAMN")
            {
                return db.GtEcmamns
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.MainMenuId,
                        FieldDesc = r.MainMenu
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECSBMN")
            {
                return db.GtEcsbmns
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.MenuItemId,
                        FieldDesc = r.MenuItemName
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECBSLN")
            {
                return db.GtEcbslns
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.BusinessKey,
                        FieldDesc = r.LocationDescription
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECCNCD")
            {
                return db.GtEccncds
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.Isdcode,
                        FieldDesc = r.CountryName
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECSTRM")
            {
                return db.GtEcstrms
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.StoreCode,
                        FieldDesc = r.StoreDesc
                    });
            }
            return null;
        }
        #endregion
    }
}
