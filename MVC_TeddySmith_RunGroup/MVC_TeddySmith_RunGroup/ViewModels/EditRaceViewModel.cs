﻿using MVC_TeddySmith_RunGroup.Data.Enum;
using MVC_TeddySmith_RunGroup.Models;

namespace MVC_TeddySmith_RunGroup.ViewModels
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? Url { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
    }
}
