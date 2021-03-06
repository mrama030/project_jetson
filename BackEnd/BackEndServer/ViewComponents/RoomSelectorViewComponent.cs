﻿using BackEndServer.Models.ViewModels;
using BackEndServer.Services.AbstractServices;
using Microsoft.AspNetCore.Mvc;

namespace BackEndServer.ViewComponents
{
    public class RoomSelectorViewComponent : ViewComponent
    {
        private AbstractLocationService _locationService;
        private AbstractLocationService LocationService => _locationService ?? (_locationService =
                                                               HttpContext.RequestServices.GetService(typeof(AbstractLocationService)) as
                                                                   AbstractLocationService);
        
        public IViewComponentResult Invoke(RoomSelectorInfo selectorInfo)
        {
            selectorInfo.Rooms = LocationService.GetRoomsAtLocation(selectorInfo.LocationId);
            return View("RoomSelector", selectorInfo);
        }
    }
}