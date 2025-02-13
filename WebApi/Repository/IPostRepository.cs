﻿using WebApi.Dto.Post;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IPostRepository
    {
        public Task<OperationResultNew> GetPostDetailsAsync(int postId, string userId, PageInfo pageInfo);
        public Task<OperationResult> GetPostByIdAsync(int postId, PageInfo pageInfo);
        public Task<OperationResult> GetPostsByUsernameAsync (string userId, PageInfo pageInfo);
        public Task<OperationResult> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo);
        public Task<OperationResult> GetPostHistoryByPostId(int postId, PageInfo pageInfo);
        public Task<OperationResult> CreatePostAsync(string userId, CreatePostDto createPostDto);
        public Task<OperationResult> UpdatePostAsync(string userId, UpdatePostDto updatePostDto);
        public Task<OperationResult> DeletePostAsync(int postId, string userId);
    }
}
