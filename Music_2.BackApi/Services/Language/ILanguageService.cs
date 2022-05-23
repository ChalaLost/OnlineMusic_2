﻿using Music_2.Data.Models;
using Music_2.Data.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music_2.BackApi.Services.Language
{
    public interface ILanguageService
    {
        /// <summary>
        /// Lấy ds ngôn ngữ
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
