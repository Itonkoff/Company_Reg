﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Company_Reg.Models;
using Company_Reg.Database;
using LinqToDB;
using System.Diagnostics;

// F

namespace Company_Reg.Controllers
{
    [Route("api/v1")]
    public class searchController : Controller
    {

        [HttpPost("postSearch")]
        public JsonResult postNewSearch([FromBody] mSearch NewSearch)
        {
            try
            {

                using (var db = new db())
                {
                    var SearchInfo = (from trans in db.SearchInfo
                                      where trans.search_ID == NewSearch.searchInfo.search_ID
                                      select trans).ToList();
                    if (SearchInfo.Count == 0)
                    {
                        NewSearch.searchInfo.SearchDate = DateTime.Now.ToString("dd/MM/yyyy");

                        db.Insert(NewSearch.searchInfo);
                    }
                    else
                    {
                        db.Update(NewSearch.searchInfo);
                    }
                    foreach (mSearchNames item in NewSearch.SearchNames)
                    {
                        var SearchDetails = (from transs in db.SearchDetails
                                             where transs.Name == item.Name
                                             select transs).FirstOrDefault();

                        if (SearchDetails == null)
                        {
                            var SearchDetailsupdate1 = (from transs in db.SearchDetails
                                                        where transs.Name_ID == item.Name_ID
                                                        select transs).FirstOrDefault();
                            if (SearchDetailsupdate1 != null)
                            {
                                item.Status = "Pending";
                                item.Search_ID = NewSearch.searchInfo.search_ID;
                                db.Update(item);
                            }
                            else
                            {
                                item.Status = "Pending";
                                item.Search_ID = NewSearch.searchInfo.search_ID;
                                db.Insert(item);
                            }

                        }
                        else
                        {
                            var SearchDetailsupdate = (from transs in db.SearchDetails
                                                       where transs.Name == item.Name && transs.Name_ID == item.Name_ID
                                                       select transs).FirstOrDefault();
                            if (SearchDetailsupdate != null)
                            {
                                db.Update(item);
                            }

                        }

                    }

                }

                return Json(new
                {
                    res = "ok",
                    id = NewSearch.searchInfo.search_ID,
                    msg = "Successfully Added  Search",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("postTask")]
        public JsonResult postNewTask([FromBody] mTasks Tasks)
        {
            try
            {

                using (var db = new db())
                {
                    var Allrecords = (from trans in db.SearchInfo
                                      where trans.Satus == "Pending"
                                      select trans).ToList();
                    if (Allrecords.Count >= Tasks.NoOfRecords)
                    {
                        var SearchInfo = (from trans in db.taskss
                                          where trans._id == Tasks._id
                                          select trans).ToList();
                        if (SearchInfo.Count == 0)
                        {
                            Tasks.Status = "Pending";
                            Tasks.Date = DateTime.Now.ToString("dd/MM/yyyy");
                            db.Insert(Tasks);
                        }
                        else
                        {
                            db.Update(Tasks);
                        }


                        var records = (from trans in db.SearchInfo
                                       where trans.Satus == "Pending"
                                       select trans).Take(Tasks.NoOfRecords).ToList();

                        if (records.Count > 0)
                        {
                            foreach (mSearchInfo info in records)
                            {
                                info.ExamanerTaskID = Tasks._id;
                                info.Examiner = Tasks.AssignTo;
                                info.Satus = "Assigned";
                                db.Update(info);
                            }
                        }

                    }
                    else
                    {
                        return Json(new
                        {
                            res = "err",
                            msg = "No of allocated Records is greater than available records"

                        });
                    }

                }




                return Json(new
                {
                    res = "ok",
                    id = Tasks._id,
                    msg = "Successfully Added Task",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpGet("UpdateTask")]
        public JsonResult UpdateTask(string  TasksID)
        {
            try
            {
                TasksID = TasksID.Trim();
                using (var db = new db())
                {
                    var Allrecords = (from trans in db.taskss
                                      where  trans._id == TasksID
                                      select trans).FirstOrDefault();
                   if(Allrecords!= null)
                    {
                        Allrecords.Status = "Completed";
                        db.Update(Allrecords);
                    }
                    else
                    {
                        return Json(new
                        {
                            res = "err",
                            msg = "Invalid TaskID",
                            taskid = TasksID


                        });
                    }

                }




                return Json(new
                {
                    res = "ok",
                    
                    msg = "Successfully Update Task",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message
                  

                });
            }

        }

        [HttpGet("SubmitSearch")]
        public JsonResult SubmitNewSearch(string tempsearchID)
        {
            try
            {
                var applicationID = "";
               mSearchInfo SearchInfo = null;
                using (var db = new db())
                {
                    SearchInfo = (from trans in db.SearchInfo
                                      where trans.search_ID == tempsearchID
                                      select trans).FirstOrDefault();

                    SearchInfo.Satus = "Pending";
                    SearchInfo.Examiner = "";
                    SearchInfo.SearchDate = DateTime.Now.ToString("dd/MM/yyyy");

                    var Ref = (from transs in db.ApplicationRef

                               select transs).FirstOrDefault();
                    SearchInfo.SearchRef = Ref.LastApplicationRef;
                    applicationID =  Ref.LastApplicationRef;
                    db.Update(SearchInfo);
                    int last = int.Parse(Ref.LastApplicationRef.ToString()) + 1;
                    Ref.LastApplicationRef = last.ToString();
                    db.Update(Ref);
                    mApplications Applications = new mApplications();

                    Applications.ApplicationID = applicationID;
                    Applications.DateOfApplication = SearchInfo.SearchDate;
                    Applications.ServiceApplied = "NAME SEARCH";
                    Applications.Status = SearchInfo.Satus;
                    Applications.Payment = 1;
                    Applications.AppliedBy = SearchInfo.search_ID;

                    db.Insert(Applications);

                }

                return Json(new
                {
                    res = "ok",
                    data = SearchInfo,
                    msg = "Successfully Added  Submitted",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }
        [HttpPost("PayforSearch")]
        public JsonResult PayForSearch([FromBody] string search_ID)
        {
            try
            {
                var applicationID = "";
                using (var db = new db())
                {
                    var SearchInfo = (from trans in db.SearchInfo
                                      where trans.search_ID == search_ID
                                      select trans).FirstOrDefault();

                    SearchInfo.Payment = "Paid";
                  
                   
                    db.Update(SearchInfo);
                   

                }

                return Json(new
                {
                    res = "ok",
                    id = applicationID,
                    msg = "Successfully Added  Submitted",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("ApproveSearchedName")]
        public JsonResult ApproveSearchedName([FromBody] mSearchNames searchNames)
        {
            try
            {

                using (var db = new db())
                {

                    var DirInfo = (from Search in db.SearchDetails
                                   where Search.Name_ID == searchNames.Name_ID
                                   select Search).FirstOrDefault();

                    if (DirInfo != null)
                    {
                        DirInfo.Status = searchNames.Status;
                        db.Update(DirInfo);
                    }

                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Updated Search",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("ApproveSearched")]
        public JsonResult ApproveSearched([FromBody] mSearchInfo search)
        {
            try
            {

                using (var db = new db())
                {

                    var DirInfo = (from Search in db.SearchInfo
                                   where Search.search_ID == search.search_ID
                                   select Search).FirstOrDefault();

                    if (DirInfo != null)
                    {
                        DirInfo.Satus = search.Satus;
                        DirInfo.ApprovedDate = search.ApprovedDate;
                        DirInfo.Satus = search.Satus;
                        db.Update(DirInfo);
                    }

                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Updated Search",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("AssignSearchForExamine")]
        public JsonResult AssignSearchForExamine([FromBody] mSearch search)
        {
            try
            {

                using (var db = new db())
                {

                    var DirInfo = (from Search in db.SearchInfo
                                   where Search.search_ID == search.searchInfo.search_ID
                                   select Search).FirstOrDefault();

                    if (DirInfo != null)
                    {
                        DirInfo.Examiner = search.searchInfo.Examiner;
                        db.Update(DirInfo);
                    }

                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Updated Search",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }


        [HttpPost("AssignCompanyForExamination")]
        public JsonResult AssignCompanyForExamination([FromBody] mCompany Company)
        {
            try
            {

                using (var db = new db())
                {

                    var DirInfo = (from Search in db.CompanyInfo
                                   where Search.Application_Ref == Company.CompanyInfo.Application_Ref
                                   select Search).FirstOrDefault();

                    if (DirInfo != null)
                    {
                        DirInfo.Examiner = Company.CompanyInfo.Application_Ref;
                        db.Update(DirInfo);
                    }

                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Updated",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("ApproveCompanyApplication")]
        public JsonResult ApproveCompanyApplication([FromBody] mCompany Company)
        {
            try
            {

                using (var db = new db())
                {

                    var DirInfo = (from Search in db.CompanyInfo
                                   where Search.Application_Ref == Company.CompanyInfo.Application_Ref
                                   select Search).FirstOrDefault();

                    if (DirInfo != null)
                    {
                        DirInfo.RegNumber = Guid.NewGuid().ToString();
                        DirInfo.Status = Company.CompanyInfo.Status;
                        db.Update(DirInfo);
                    }

                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Updated",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpGet("GetNameSearches")]
        public JsonResult GetNameSearches()
        {
            try
            {
                var db = new db();
                List<mSearch> searched = new List<mSearch>();
                var SearchInfo = (from trans in db.SearchInfo

                                  select trans).ToList();
                foreach (mSearchInfo info in SearchInfo)
                {
                    var SearchDetails = (from transs in db.SearchDetails
                                         where transs.Search_ID == info.search_ID
                                         select transs).ToList();
                    searched.Add(new mSearch { searchInfo = info, SearchNames = SearchDetails });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(searched)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("CheckName")]
        public JsonResult CheckName(string name)
        {
            try
            {
                var db = new db();

                var SearchDetails = (from transs in db.SearchDetails
                                     where transs.Name == name
                                     select transs).FirstOrDefault();

                if (SearchDetails == null)
                {
                    return Json(new
                    {
                        res = "ok",
                        msg = "Name Available"
                    });
                }
                else
                {
                    return Json(new
                    {
                        res = "err",
                        msg = "Name Not Available"
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("CheckNameStartWithInNameSearch")]
        public JsonResult CheckNameS(string name)
        {
            try
            {
                var db = new db();

                var SearchDetails = (from transs in db.SearchDetails
                                     where transs.Status == "Pending" || transs.Status == "Reserved" || transs.Status == "Registered"
                                     select transs).ToList();
                // var resultList = SearchDetails.Where(x => x.Name.Contains("abc")).ToList();
                if (SearchDetails.Count > 1)
                {
                    // var resultStartWith = SearchDetails.Where(x => x.Name.StartsWith(name)).ToList();
                    var resultStartWith = SearchDetails.Where(x => x.Name == name).ToList();

                    if (resultStartWith.Count > 0)
                    {
                        return Json(new
                        {
                            res = "ok",
                            msg = resultStartWith
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            res = "err",
                            msg = "Nothing start with this name"
                        });
                    }

                }

                else
                {
                    return Json(new
                    {
                        res = "err",
                        msg = "Nothing start with this name"
                    });
                }


            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("CheckNameStartContainsInNameSearch")]
        public JsonResult CheckNameContains(string name)
        {
            try
            {
                var db = new db();

                var SearchDetails = (from transs in db.SearchDetails
                                     where transs.Status == "Pending" || transs.Status == "Reserved" || transs.Status == "Registered"
                                     select transs).ToList();
                var resultListContains = SearchDetails.Where(x => x.Name == name).ToList();
                //var resultStartWith = SearchDetails.Where(x => x.Name.StartsWith(name)).ToList();

                if (resultListContains.Count > 0)
                {
                    return Json(new
                    {
                        res = "ok",
                        msg = resultListContains
                    });
                }
                else
                {
                    return Json(new
                    {
                        res = "err",
                        msg = "Database Registry does not have anything that start with " + name
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetNameSearchesByUser")]
        public JsonResult GetNameSearchesByUser(string UserID)
        {
            try
            {
                var db = new db();
                List<mSearch> searched = new List<mSearch>();
                var SearchInfo = (from trans in db.SearchInfo
                                  where trans.Searcher_ID == UserID
                                  select trans).ToList();
                foreach (mSearchInfo info in SearchInfo)
                {
                    var SearchDetails = (from transs in db.SearchDetails
                                         where transs.Search_ID == info.search_ID
                                         select transs).ToList();
                    searched.Add(new mSearch { searchInfo = info, SearchNames = SearchDetails });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(searched)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetNameSearchesByUserByTaskID")]
        public JsonResult GetNameSearchesByUserByTask(string UserID, string TaskID)
        {
            try
            {
                var db = new db();
                List<mSearch> searched = new List<mSearch>();
                var SearchInfo = (from trans in db.SearchInfo
                                  where trans.Examiner == "Examiner 1" && trans.ExamanerTaskID == TaskID
                                  select trans).ToList();
                foreach (mSearchInfo info in SearchInfo)
                {
                    var SearchDetails = (from transs in db.SearchDetails
                                         where transs.Search_ID == info.search_ID
                                         select transs).ToList();
                    searched.Add(new mSearch { searchInfo = info, SearchNames = SearchDetails });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(searched)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetNameSearchesBySearchID")]
        public JsonResult GetNameSearchesBySearchID(string ID)
        {
            try
            {
                var db = new db();
                List<mSearch> searched = new List<mSearch>();
                var SearchInfo = (from trans in db.SearchInfo
                                  where trans.search_ID == ID
                                  select trans).ToList();
                foreach (mSearchInfo info in SearchInfo)
                {
                    var SearchDetails = (from transs in db.SearchDetails
                                         where transs.Search_ID == info.search_ID
                                         select transs).ToList();
                    searched.Add(new mSearch { searchInfo = info, SearchNames = SearchDetails });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(searched)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetNameSearchesByExaminerID")]
        public JsonResult GetNameSearchesByExaminerID(string ID)
        {
            try
            {
                var db = new db();
                List<mSearch> searched = new List<mSearch>();
                var SearchInfo = (from trans in db.SearchInfo
                                  where trans.Examiner == ID
                                  select trans).ToList();
                foreach (mSearchInfo info in SearchInfo)
                {
                    var SearchDetails = (from transs in db.SearchDetails
                                         where transs.Search_ID == info.search_ID
                                         select transs).ToList();
                    searched.Add(new mSearch { searchInfo = info, SearchNames = SearchDetails });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(searched)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpPost("postCompanyApplication")]
        public JsonResult postCompanyApplication([FromBody] mCompany NewCompany)
        {
            try
            {
                //NewCompany.CompanyInfo.Application_Ref = Guid.NewGuid().ToString();
                NewCompany.CompanyInfo.Status = "Pending";
                using (var db = new db())
                {



                    var CoInfo = (from Dir in db.CompanyInfo
                                  where Dir.Application_Ref == NewCompany.CompanyInfo.Application_Ref
                                  select Dir).FirstOrDefault();
                    if (CoInfo == null)
                    {
                        db.Insert(NewCompany.CompanyInfo);

                    }
                    else
                    {
                        if (NewCompany.CompanyInfo.step == 2)
                        {
                            CoInfo.Registered_Address = NewCompany.CompanyInfo.Registered_Address;
                            CoInfo.City = NewCompany.CompanyInfo.City;
                            CoInfo.Country = NewCompany.CompanyInfo.Country;
                            CoInfo.Email = NewCompany.CompanyInfo.Email;
                            CoInfo.step = NewCompany.CompanyInfo.step;
                            db.Update(CoInfo);
                        }
                       
                    }
                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Added  Company Registration Application",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("SubmitCompanyApplication")]
        public JsonResult SubmitCompanyApplication([FromBody] mCompany NewCompany)
        {
            try
            {
                //NewCompany.CompanyInfo.Application_Ref = Guid.NewGuid().ToString();
                
                using (var db = new db())
                {

                    var CoInfo = (from Dir in db.CompanyInfo
                        where Dir.Application_Ref == NewCompany.CompanyInfo.Application_Ref
                        select Dir).FirstOrDefault();
                    if (CoInfo != null)
                    {
                        CoInfo.Status = "Submitted";

                        db.Update(CoInfo);
                    }
                    
                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Added  Company Registration Application",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }
        [HttpPost("PaidCompanyApplication")]
        public JsonResult PaidCompanyApplication([FromBody] mCompany NewCompany)
        {
            try
            {
                //NewCompany.CompanyInfo.Application_Ref = Guid.NewGuid().ToString();
                
                using (var db = new db())
                {



                    var CoInfo = (from Dir in db.CompanyInfo
                        where Dir.Application_Ref == NewCompany.CompanyInfo.Application_Ref
                        select Dir).FirstOrDefault();
                    CoInfo.Payment = "Paid";
                    if (CoInfo != null)
                    {
                        db.Update(CoInfo);

                    }
                    
                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Submitted  Company Registration Application",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }
        [HttpPost("postCompanyApplicationMemo")]
        public JsonResult postCompanyApplicationMemo([FromBody] PostMemo NewMemo)
        {
            try
            {
                //NewCompany.CompanyInfo.Application_Ref = Guid.NewGuid().ToString();
              
                using (var db = new db())
                {



                    var CoInfo = (from Dir in db.CompanyInfo
                                  where Dir.Application_Ref == NewMemo.memo.Application_Ref
                                  select Dir).FirstOrDefault();
                    if (CoInfo != null)
                    {
                        var CoInfoMemo = (from Dirm in db.memo
                                      where Dirm._id == NewMemo.memo._id
                                      select Dirm).FirstOrDefault();
                         //to keep track of the step in coinfo
                        CoInfo.step = NewMemo.step;
                        db.Update(CoInfo);
                        if (CoInfoMemo == null)
                        {
                            db.Insert(NewMemo.memo);
                        }
                        else
                        {
                            db.Update(NewMemo.memo);
                        }

                        var CoMemoObjects = (from Diro in db.objects
                                      where Diro.memo_id == NewMemo.memo._id
                                      select Diro).ToList();
                        if (CoMemoObjects.Count==0)
                        {
                            foreach(mmainClause obj in NewMemo.memo.objects)

                            {

                                db.Insert(obj);
                            }   

                        }
                        else
                        {
                            foreach (mmainClause obj in NewMemo.memo.objects)

                            {
                                var CoObjects = (from Diro in db.objects
                                                     where Diro.memo_id == obj._id
                                                     select Diro).ToList();
                                if (CoObjects.Count == 0)
                                {
                                    db.Insert(obj);
                                }
                                else
                                {
                                    db.Update(obj);
                                }
                                
                            }
                        }

                    }
                    else
                    {
                        return Json(new
                        {
                            res = "err",
                            msg = "Invalid Company Application Ref"

                        });
                    }
                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Added  Company Registration Application",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("postCompanyApplicationArticles")]
        public JsonResult postCompanyApplicationArticles([FromBody] postArticles NewArticles)
        {
            try
            {
               

                using (var db = new db())
                {
                    

                    var CoInfo = (from Dir in db.CompanyInfo
                                  where Dir.Application_Ref == NewArticles.Articles.Application_Ref
                                  select Dir).FirstOrDefault();
                    if (CoInfo != null)
                    {
                        var CoArt = (from Dir in db.articles
                                      where Dir._id == NewArticles.Articles._id
                                      select Dir).FirstOrDefault();
                        CoInfo.step = NewArticles.step;
                        db.Update(CoInfo);
                        if (CoArt == null)
                        {
                            db.Insert(NewArticles.Articles);
                        }

                        else
                        {
                            db.Update(NewArticles.Articles);
                        }

                    }
                    else
                    {
                        return Json(new
                        {
                            res = "err",
                            msg = "Invalid Company Application Ref"

                        });
                    }
                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Added  Company Articles",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }

        [HttpPost("postCompanyApplicationMembers")]
        public JsonResult postCompanyApplicationMembers([FromBody] PostMembers NewMembers)
        {
            try
            {


                using (var db = new db())
                {
                    foreach(mMembersInfo mb in NewMembers.members)
                    {
                        var CoMembers = (from Dir in db.MembersInfo
                                         where Dir.ID_No == mb.ID_No
                                         select Dir).FirstOrDefault();
                        if (CoMembers == null)
                        {
                            db.Insert(mb);
                        }
                        else
                        {
                            db.Update(mb);
                        }
                    }
                   
                    foreach( mMembersPotifolio mbp in NewMembers.membersPotifolio)
                    {
                        var CoInfo = (from Dir in db.CompanyInfo
                                      where Dir.Application_Ref ==mbp.Application_Ref
                                      select Dir).FirstOrDefault();
                        if (CoInfo != null)
                        {

                            CoInfo.step = NewMembers.step;
                            db.Update(CoInfo);

                            var CoMePort = (from Dir in db.MembersPortifolio
                                            where Dir.company_member_id == mbp.company_member_id
                                            select Dir).FirstOrDefault();
                            if (CoMePort == null)
                            {
                                db.Insert(mbp);
                            }

                            else
                            {
                                db.Update(mbp);
                            }

                        }

                        else
                        {
                            return Json(new
                            {
                                res = "err",
                                msg = "Invalid Company Application Ref"

                            });
                        }
                    }

                   
                    
                }

                return Json(new
                {
                    res = "ok",
                    msg = "Successfully Added  Members",

                });
            }

            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    msg = ex.Message

                });
            }

        }
        [HttpGet("GetCompanyApplication")]
        public JsonResult GetCompanyApplication()
        {
            try
            {
                var db = new db();
                List<mCompany> Applications = new List<mCompany>();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       select trans).ToList();
                foreach (mCompanyInfo info in ApplicationInfo)
                {
                    var Directors = (from transs in db.DirectorsPortifolio
                                     where transs.Application_Ref == info.Application_Ref
                                     select transs).ToList();
                    List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();
                    foreach (mDirectorsPotifolio dirinfo in Directors)
                    {
                        var DirInformation = (from transsInfo in db.DirectorInfo
                                              where transsInfo.director_id == dirinfo.director_id
                                              select transsInfo).FirstOrDefault();
                        DirectorsInfomation.Insert(0, DirInformation);
                    }

                    Applications.Add(new mCompany { CompanyInfo = info });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetCompanyApplicationBySearcRef")]
        public JsonResult GetCompanyApplicationBySearcRef(string SearchRef)
        {
            try
            {
                var db = new db();
                mCompany Applications = new mCompany();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       where trans.Search_Ref == SearchRef
                                       select trans).FirstOrDefault();

                var Directors = (from transs in db.DirectorsPortifolio
                                 where transs.Application_Ref == ApplicationInfo.Application_Ref
                                 select transs).ToList();
                List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();

                mMemorandum memos = new mMemorandum();
                var mem = (from trans in db.memo
                           where trans.Application_Ref == ApplicationInfo.Application_Ref
                           select trans).FirstOrDefault();
                List<mmainClause> objects = new List<mmainClause>();
                if (memos != null)
                {
                    var memoobjects = (from trans in db.objects
                                     where trans.memo_id == mem._id
                                     select trans).ToList();
                    mem.objects = memoobjects;
                }

                List<mMembersPotifolio> MembersPotifolios = new List<mMembersPotifolio>();
                var members = (from transs in db.MembersPortifolio
                               where transs.Application_Ref == ApplicationInfo.Application_Ref
                               select transs).ToList();

                List<mMembersInfo> memberss = new List<mMembersInfo>();
                if (members.Count > 0) {
     
                    foreach (mMembersPotifolio mp in members)
                    {
                        memberss.Add((from transs in db.MembersInfo
                                      where transs.member_id == mp.member_id
                                      select transs).FirstOrDefault());
                    }
                }



                mArticles articles = new mArticles();
       
                var articless = (from transs in db.articles
                      where transs.Application_Ref == ApplicationInfo.Application_Ref
                                 select transs).FirstOrDefault();
                Applications.members = memberss;
                Applications.MembersPotifolios = members;
                Applications.memo = mem;

                Applications.articles = articless;
                Applications.CompanyInfo = ApplicationInfo;
             
                  

               

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetCompanyApplicationByUserID")]
        public JsonResult GetCompanyApplicationByUserID(string UserID)
        {
            try
            {
                var db = new db();
               List<mCompany> Applicationss = new List<mCompany>();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       where trans.AppliedBy == UserID
                                       select trans).ToList();
                foreach (var co in ApplicationInfo)
                {
                    mCompany Applications = new mCompany();
                    mMemorandum memos = new mMemorandum();
                    var mem = (from trans in db.memo
                        where trans.Application_Ref == co.Application_Ref
                        select trans).FirstOrDefault();

                    if (mem != null)
                    {
                        List<mmainClause> objects = new List<mmainClause>();
                        var memojects = (from trans in db.objects
                            where trans.memo_id == mem._id
                            select trans).ToList();

                        mem.objects = memojects;
                    }


                    List<mMembersPotifolio> MembersPotifolios = new List<mMembersPotifolio>();
                    var members = (from transs in db.MembersPortifolio
                        where transs.Application_Ref == co.Application_Ref
                        select transs).ToList();
                    List<mMembersInfo> memberss = new List<mMembersInfo>();
                    foreach (mMembersPotifolio mp in members)
                    {
                        memberss.Add((from transs in db.MembersInfo
                            where transs.member_id == mp.member_id
                            select transs).FirstOrDefault());
                    }


                    mArticles articles = new mArticles();

                    var articless = (from transs in db.articles
                        where transs.Application_Ref == co.Application_Ref
                        select transs).FirstOrDefault();
                    Applications.members = memberss;
                    Applications.MembersPotifolios = members;
                    Applications.memo = mem;

                    Applications.articles = articless;
                    Applications.CompanyInfo = co;

                    Applicationss.Add(Applications);
                }
               

               





                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applicationss)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetCompanyApplicationByStatus")]
        public JsonResult GetCompanyApplicationByStatus(string status)
        {
            try
            {
                var db = new db();
                List<mCompany> Applications = new List<mCompany>();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       where trans.Status == status
                                       select trans).ToList();
                foreach (mCompanyInfo info in ApplicationInfo)
                {
                    var Directors = (from transs in db.DirectorsPortifolio
                                     where transs.Application_Ref == info.Application_Ref
                                     select transs).ToList();
                    List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();
                    foreach (mDirectorsPotifolio dirinfo in Directors)
                    {
                        var DirInformation = (from transsInfo in db.DirectorInfo
                                              where transsInfo.director_id == dirinfo.director_id
                                              select transsInfo).FirstOrDefault();
                        DirectorsInfomation.Insert(0, DirInformation);
                    }

                    Applications.Add(new mCompany { CompanyInfo = info });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetCompanyApplicationByExaminer")]
        public JsonResult GetCompanyApplicationByExaminer(string Eximiner)
        {
            try
            {
                var db = new db();
                List<mCompany> Applications = new List<mCompany>();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       where trans.Examiner == Eximiner
                                       select trans).ToList();
                foreach (mCompanyInfo info in ApplicationInfo)
                {
                    var Directors = (from transs in db.DirectorsPortifolio
                                     where transs.Application_Ref == info.Application_Ref
                                     select transs).ToList();
                    List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();
                    foreach (mDirectorsPotifolio dirinfo in Directors)
                    {
                        var DirInformation = (from transsInfo in db.DirectorInfo
                                              where transsInfo.director_id == dirinfo.director_id
                                              select transsInfo).FirstOrDefault();
                        DirectorsInfomation.Insert(0, DirInformation);
                    }

                    Applications.Add(new mCompany { CompanyInfo = info });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetCompanyApplicationByExaminerAndStatus")]
        public JsonResult GetCompanyApplicationByExaminerAndStatus(string Examiner, string Status)
        {
            try
            {
                var db = new db();
                List<mCompany> Applications = new List<mCompany>();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       where trans.Examiner == Examiner && trans.Status == Status
                                       select trans).ToList();
                foreach (mCompanyInfo info in ApplicationInfo)
                {
                    var Directors = (from transs in db.DirectorsPortifolio
                                     where transs.Application_Ref == info.Application_Ref
                                     select transs).ToList();
                    List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();
                    foreach (mDirectorsPotifolio dirinfo in Directors)
                    {
                        var DirInformation = (from transsInfo in db.DirectorInfo
                                              where transsInfo.director_id == dirinfo.director_id
                                              select transsInfo).FirstOrDefault();
                        DirectorsInfomation.Insert(0, DirInformation);
                    }

                    Applications.Add(new mCompany { CompanyInfo = info });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetCompanyApplicationByApplicationRef")]
        public JsonResult GetCompanyApplicationByApplicationRef(string ApplicationRef)
        {
            try
            {
                var db = new db();
                List<mCompany> Applications = new List<mCompany>();
                var ApplicationInfo = (from trans in db.CompanyInfo
                                       where trans.Application_Ref == ApplicationRef
                                       select trans).ToList();
                foreach (mCompanyInfo info in ApplicationInfo)
                {
                    var Directors = (from transs in db.DirectorsPortifolio
                                     where transs.Application_Ref == info.Application_Ref
                                     select transs).ToList();
                    List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();
                    foreach (mDirectorsPotifolio dirinfo in Directors)
                    {
                        var DirInformation = (from transsInfo in db.DirectorInfo
                                              where transsInfo.director_id == dirinfo.director_id
                                              select transsInfo).FirstOrDefault();
                        DirectorsInfomation.Insert(0, DirInformation);
                    }

                    Applications.Add(new mCompany { CompanyInfo = info });

                }

                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetCompanyApplicationByDirectorID")]
        public JsonResult GetCompanyApplicationByDirectorID(string ID)
        {
            try
            {
                var db = new db();
                List<mCompany> Applications = new List<mCompany>();


                var Directors = (from transs in db.DirectorsPortifolio
                                     // where transs.ID_No == ID


                                 select transs).GroupBy(q => q.Application_Ref).ToList();


                foreach (mDirectorsPotifolio dp in Directors)
                {


                    var ApplicationInfo = (from trans in db.CompanyInfo
                                           where trans.Application_Ref == dp.Application_Ref
                                           select trans).FirstOrDefault();
                    var SpecDirectors = (from SpecDir in db.DirectorsPortifolio
                                         where SpecDir.Application_Ref == dp.Application_Ref

                                         select SpecDir).ToList();

                    List<mDirectorInfo> DirectorsInfomation = new List<mDirectorInfo>();
                    foreach (mDirectorsPotifolio dirinfo in SpecDirectors)
                    {
                        var DirInformation = (from transsInfo in db.DirectorInfo
                                                  // where transsInfo.ID_No == dirinfo.ID_No
                                              select transsInfo).FirstOrDefault();
                        DirectorsInfomation.Insert(0, DirInformation);
                    }
                    Applications.Add(new mCompany { CompanyInfo = ApplicationInfo });
                }








                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(Applications)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetApplication")]
        public JsonResult GetApplication(string ID)
        {
            try
            {
                var db = new db();
                List<mApplications> Applications = new List<mApplications>();


                var AllApplication = (from transs in db.applications
                                      where transs.AppliedBy == ID

                                      select transs).ToList();




                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(AllApplication)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
        [HttpGet("GetAllApplication")]
        public JsonResult GetAllApplication()
        {
            try
            {
                var db = new db();
                List<mApplications> Applications = new List<mApplications>();


                var AllApplication = (from transs in db.applications

                                      select transs).ToList();


                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(AllApplication)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetAllTasks")]
        public JsonResult GetAllTasks()
        {
            try
            {
                var db = new db();
                List<mTasks> Tasks = new List<mTasks>();


                var AllTasks = (from transs in db.taskss

                                select transs).ToList();


                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(AllTasks)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetTasksByExaminer")]
        public JsonResult GetTasksByExaminer(string UserID)
        {
            try
            {
                var db = new db();
                List<mTasks> Tasks = new List<mTasks>();


                var AllTasks = (from transs in db.taskss
                                where transs.AssignTo == UserID
                                select transs).ToList();


                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(AllTasks)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }

        [HttpGet("GetTasksByPExaminer")]
        public JsonResult GetTasksByPExaminer(string UserID)
        {
            try
            {
                var db = new db();
                List<mTasks> Tasks = new List<mTasks>();


                var AllTasks = (from transs in db.taskss
                                where transs.Assigner == UserID
                                select transs).ToList();


                //return success
                return Json(new
                {
                    res = "ok",
                    data = Json(AllTasks)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    res = "err",
                    data = ex.Message
                });
            }

        }
    }


}