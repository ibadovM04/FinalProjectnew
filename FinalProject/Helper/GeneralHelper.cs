﻿using FinalProject.Data;
using FinalProject.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Commercio.Helper
{
    public static class GeneralHelper
    {
        public static async Task<List<CategoryDto>> GetCategoryTree(ApplicationDbContext _context, int? parentId = null)
        {

            
            List<CategoryDto> result = new List<CategoryDto>();

            var categories = await _context.Categories.Where(c => c.ParentId == parentId)
                                                        .Select(c => new CategoryDto
                                                        {
                                                            CategoryId = c.Id,
                                                            Slogan = c.Slogan,
                                                            ImageUrl = c.ImageUrl,
                                                            Name = c.Name,
                                                            ParentId = c.ParentId,
                                                            Priority = c.Priority ?? 0
                                                        })
                                                        .ToListAsync();
            foreach (var category in categories)
            {
                var children = await _context.Categories.Where(c => c.ParentId == category.CategoryId).Select(c => new CategoryDto
                {
                    CategoryId = c.Id,
                    Slogan = c.Slogan,
                    ImageUrl = c.ImageUrl,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    Priority = c.Priority ?? 0
                }).ToListAsync();

                if (children.Count > 0)
                {
                    category.Children.AddRange(await GetCategoryTree(_context, category.CategoryId));
                }

            }

            result.AddRange(categories);

            return result;


        }
    }
}
