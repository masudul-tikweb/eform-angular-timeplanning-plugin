﻿/*
The MIT License (MIT)

Copyright (c) 2007 - 2021 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Sentry;

namespace TimePlanning.Pn.Services.TimePlanningPlanningService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models.Planning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microting.eForm.Infrastructure.Constants;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.TimePlanningBase.Infrastructure.Data;
using Microting.TimePlanningBase.Infrastructure.Data.Entities;
using TimePlanningLocalizationService;

public class TimePlanningPlanningService(
    ILogger<TimePlanningPlanningService> logger,
    TimePlanningPnDbContext dbContext,
    IUserService userService,
    ITimePlanningLocalizationService localizationService,
    IEFormCoreService core)
    : ITimePlanningPlanningService
{
    public async Task<OperationDataResult<List<TimePlanningPlanningModel>>> Index(
        TimePlanningPlanningRequestModel model)
    {
        try
        {
            //         var timePlanningRequest = dbContext.PlanRegistrations
            //             .Where(x => x.WorkflowState != Constants.WorkflowStates.Removed);
            //             //.Where(x => x.SdkSitId == model.SiteId);
            //
            //         // two dates may be displayed instead of one if the same date is selected.
            //         if (model.DateFrom == model.DateTo)
            //         {
            //             timePlanningRequest = timePlanningRequest
            //                 .Where(x => x.Date == model.DateFrom);
            //         }
            //         else
            //         {
            //             timePlanningRequest = timePlanningRequest
            //                 .Where(x => x.Date >= model.DateFrom && x.Date <= model.DateTo);
            //         }
            //
            //         var timePlannings = await timePlanningRequest
            //             .Select(x => new TimePlanningPlanningHelperModel
            //             {
            //                 WeekDay = (int)x.Date.DayOfWeek,
            //                 Date = x.Date,
            //                 PlanText = x.PlanText,
            //                 PlanHours = x.PlanHours,
            //                 Message = x.MessageId
            //             })
            //             .ToListAsync();
            //
            //         var date = (int)(model.DateTo - model.DateFrom).TotalDays + 1;
            //
            //         if (timePlannings.Count < date)
            //         {
            //             var daysForAdd = new List<TimePlanningPlanningHelperModel>();
            //             for (var i = 0; i < date; i++)
            //             {
            //                 if (timePlannings.All(x => x.Date != model.DateFrom.AddDays(i)))
            //                 {
            //                     daysForAdd.Add(new TimePlanningPlanningHelperModel
            //                     {
            //                         Date = model.DateFrom.AddDays(i),
            //                         WeekDay = (int)model.DateFrom.AddDays(i).DayOfWeek
            //                     });
            //                 }
            //             }
            //
            //             timePlannings.AddRange(daysForAdd);
            //         }
            //
            //         if (model.Sort.ToLower() == "dayofweek")
            //         {
            //             List<TimePlanningPlanningHelperModel> tempResult;
            //
            //             if (model.IsSortDsc)
            //             {
            //                 tempResult = timePlannings
            //                     .Where(x => x.WeekDay == 0)
            //                     .OrderByDescending(x => x.WeekDay)
            //                     .ThenByDescending(x => x.Date)
            //                     .ToList();
            //                 tempResult.AddRange(timePlannings
            //                     .Where(x => x.WeekDay > 0)
            //                     .OrderByDescending(x => x.WeekDay));
            //             }
            //             else
            //             {
            //                 tempResult = timePlannings
            //                     .Where(x => x.WeekDay > 0)
            //                     .OrderBy(x => x.WeekDay)
            //                     .ThenBy(x => x.Date)
            //                     .ToList();
            //                 tempResult.AddRange(timePlannings
            //                     .Where(x => x.WeekDay == 0)
            //                     .OrderBy(x => x.Date));
            //             }
            //
            //             timePlannings = tempResult;
            //         }
            //         else
            //         {
            //             timePlannings = model.IsSortDsc
            //                 ? timePlannings.OrderByDescending(x => x.Date).ToList()
            //                 : timePlannings.OrderBy(x => x.Date).ToList();
            //         }
            //
            //         var result = timePlannings
            //             .Select(x => new TimePlanningPlanningModel
            //             {
            //                 WeekDay = x.WeekDay,
            //                 Date = x.Date.ToString("yyyy/MM/dd"),
            //                 PlanText = x.PlanText,
            //                 PlanHours = x.PlanHours,
            //                 Message = x.Message,
            //                 IsWeekend = x.Date.DayOfWeek == DayOfWeek.Saturday || x.Date.DayOfWeek == DayOfWeek.Sunday
            //             })
            //             .ToList();
            //

            var result = new List<TimePlanningPlanningModel>();

            return new OperationDataResult<List<TimePlanningPlanningModel>>(
                true,
                result);
        }
        catch (Exception e)
        {
            SentrySdk.CaptureException(e);
            logger.LogError(e.Message);
            logger.LogTrace(e.StackTrace);
            return new OperationDataResult<List<TimePlanningPlanningModel>>(
                false,
                localizationService.GetString("ErrorWhileObtainingPlannings"));
        }
    }
    //
    // public async Task<OperationResult> UpdateCreatePlanning(TimePlanningPlanningUpdateModel model)
    // {
    //     try
    //     {
    //         var planning = await dbContext.PlanRegistrations
    //             .Where(x => x.WorkflowState != Constants.WorkflowStates.Removed)
    //             .Where(x => x.SdkSitId == model.SiteId)
    //             .Where(x => x.Date == model.Date)
    //             .FirstOrDefaultAsync();
    //         if (planning != null)
    //         {
    //             return await UpdatePlanning(planning, model);
    //         }
    //
    //         return await CreatePlanning(model, model.SiteId);
    //     }
    //     catch (Exception e)
    //     {
    //         SentrySdk.CaptureException(e);
    //         logger.LogError(e.Message);
    //         logger.LogTrace(e.StackTrace);
    //         return new OperationResult(
    //             false,
    //             localizationService.GetString("ErrorWhileUpdatePlanning"));
    //     }
    // }
    //
    // private async Task<OperationResult> CreatePlanning(TimePlanningPlanningUpdateModel model, int sdkSiteId)
    // {
    //     try
    //     {
    //         var planning = new PlanRegistration
    //         {
    //             PlanText = model.PlanText,
    //             SdkSitId = sdkSiteId,
    //             Date = model.Date,
    //             PlanHours = model.PlanHours,
    //             CreatedByUserId = userService.UserId,
    //             UpdatedByUserId = userService.UserId,
    //             MessageId = model.Message
    //         };
    //
    //         await planning.Create(dbContext);
    //
    //         return new OperationResult(
    //             true,
    //             localizationService.GetString("SuccessfullyCreatePlanning"));
    //     }
    //     catch (Exception e)
    //     {
    //         SentrySdk.CaptureException(e);
    //         logger.LogError(e.Message);
    //         logger.LogTrace(e.StackTrace);
    //         return new OperationResult(
    //             false,
    //             localizationService.GetString("ErrorWhileCreatePlanning"));
    //     }
    // }
    //
    // private async Task<OperationResult> UpdatePlanning(PlanRegistration planning,
    //     TimePlanningPlanningUpdateModel model)
    // {
    //     try
    //     {
    //         planning.MessageId = model.Message;
    //         planning.PlanText = model.PlanText;
    //         planning.PlanHours = model.PlanHours;
    //         planning.UpdatedByUserId = userService.UserId;
    //
    //         await planning.Update(dbContext);
    //
    //         return new OperationResult(
    //             true,
    //             localizationService.GetString("SuccessfullyUpdatePlanning"));
    //     }
    //     catch (Exception e)
    //     {
    //         SentrySdk.CaptureException(e);
    //         logger.LogError(e.Message);
    //         logger.LogTrace(e.StackTrace);
    //         return new OperationResult(
    //             false,
    //             localizationService.GetString("ErrorWhileUpdatePlanning"));
    //     }
    // }
}