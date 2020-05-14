﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZLServerDashboard.Filters;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;
using ZLServerDashboard.Models.ViewDto;

namespace ZLServerDashboard.Controllers
{
    public class UserController : BaseController
    {

        public UserController(IMapper mapper,  IUserService userService, IRoleService roleService)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.roleService = roleService;
        }



        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Index()
        {
            PowerDto dto = new PowerDto();
            var menus = userService.FindUserMenus(UserInfo, false);
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("User") && p.Url.Contains("Add")).Any())
                dto.Add = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("User") && p.Url.Contains("Edit")).Any())
                dto.Edit = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("User") && p.Url.Contains("Delete")).Any())
                dto.Delete = true;
            return View(dto);
        }

        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public TableQueryModel<UserDto> Index(QueryModel queryModel)
        {

            return userService.GetUserList(queryModel);
        }

        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Delete(long[] ids)
        {
            BaseModel<String> result = new BaseModel<string>();
            try
            {
                result = userService.DeleteUsers(ids, UserInfo);

            }
            catch (Exception ex)
            {
                result.Failed(ex);
            }
            return result;
        }

        



        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Add()
        {
            UserEditDataSourceDto dto = new UserEditDataSourceDto();




            dto.AllRoles = roleService.GetCanPowerRoles(UserInfo);

            dto.User = new UserDto();
            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Add(UserDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                baseModel = userService.AddUser(dto, UserInfo);

            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }
        


        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Edit(long id)
        {
            UserEditDataSourceDto dto = new UserEditDataSourceDto();
            var loginUserId = TokenDto.UserId;

            dto.AllRoles = roleService.GetCanPowerRoles(UserInfo);

            dto.User = userService.FindUserById(id);
            dto.User.LoginPasswd = null;
            var nowRole = roleService.FindUserRoles(dto.User).FirstOrDefault();
            dto.User.TempRoleId = nowRole?.Id;


            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Edit(UserDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {

                baseModel = userService.EditUser(dto, UserInfo);
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }
        

        [HttpGet]
        [SkipGlobalActionFilter]
        public BaseModel<String> ReSetAdminPasswd()
        {
            return userService.ResetAdminPassword();
        }
    }
}