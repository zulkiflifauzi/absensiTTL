using AbsensiTTL.Models;
using AbsensiTTL.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AbsensiTTL.App_Start.AutoMapperMaps), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AbsensiTTL.App_Start.AutoMapperMaps), "Stop")]


namespace AbsensiTTL.App_Start
{
    public static class AutoMapperMaps
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            //Mapper.CreateMap<ViewModelLogin, User>()
            //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            //.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));     
            Mapper.CreateMap<ViewModelCheckInOut, ViewCheckInOut>();
            Mapper.CreateMap<ViewCheckInOut, ViewModelCheckInOut>();
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {

        }
    }
}