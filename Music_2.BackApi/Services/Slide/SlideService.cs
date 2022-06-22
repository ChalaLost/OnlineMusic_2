﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Music_2.BackApi.Services.Common;
using Music_2.Data.EF;
using Music_2.Data.Entities;
using Music_2.Data.Models;
using Music_2.Data.Models.Slide;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Music_2.BackApi.Services.Slide
{
    public class SlideService : ISlideService
    {
        private readonly OnlineMusicDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public SlideService(OnlineMusicDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> Create(SlideCreateRequest request)
        {
            var slide = new Data.Entities.Slide()
            {
                Name = request.Name,
                Description = request.Description,
                Url = request.Url,
                Image = await this.SaveFile(request.Image),
            };
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
            return slide.Id;

        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            var query = _context.Slides;
            return await query.Select(x => new SlideViewModel()
            {
                Image = x.Image
            }).ToListAsync();
        }


        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }
    }
}
