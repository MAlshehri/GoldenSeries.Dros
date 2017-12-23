using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenSeries.Dros.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldenSeries.Dros.Services
{
    public class DataStore
    {
        readonly DrosDbContext _context;

        public DataStore()
        {
            _context = new DrosDbContext();
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _context.Authors.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Author> GetAuthor(Guid authorId)
        {
            return await _context.Authors.FirstOrDefaultAsync(x => x.Id == authorId);
        }

        public async Task<List<Material>> GetAuthorMaterials(Guid authorId)
        {
            return await _context.MaterialsAuthors
                                 .Where(x => x.AuthorId == authorId)
                                 .Select(x => x.Material)
                                 .Include(x => x.Links).Include(x => x.Tags)
                                 .ToListAsync();
        }

        public async Task<List<Tag>> GetTags()
        {
            return await _context.Tags.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Tag> GetTag(Guid tagId)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == tagId);
        }

        public async Task<List<Material>> GetTagMaterials(Guid tagId)
        {
            return await _context.MaterialsTags.Where(x => x.TagId == tagId).Select(x => x.Material).ToListAsync();
        }

        public async Task<List<Material>> GetMaterials()
        {
            return await _context.Materials.Include(x => x.Links).Include(x => x.Tags).OrderBy(x => x.Title).ToListAsync();
        }

        public async Task<List<Link>> GetMaterialLinks(Guid materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(x => x.Id == materialId);
            return material.Links.OrderBy(x => x.Order).ToList();
        }
    }
}
