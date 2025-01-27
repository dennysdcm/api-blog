using ApiBlog.Application.Dtos;
using ApiBlog.Application.Services;
using ApiBlog.Domain.Repositories;
using ApiBlog.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiBlog.WebApi.Controllers;

public class PostsController : BaseController
{
    private readonly IPostsService _postsService;

    public PostsController(IPostsService postsService)
    {
        _postsService = postsService;
    }

    [HttpGet]
    [SwaggerOperation(Description = "Retrieves a list of all posts.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(CancellationToken cancellationToken = default)
    {
        var posts = await _postsService.GetPostsAsync(cancellationToken);
        return Ok(posts);
    }
        
    [HttpGet("{id}")]
    [SwaggerOperation(Description = "Retrieves a post by Id.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPost(string id, CancellationToken cancellationToken = default)
    {
        var post = await _postsService.GetPostByIdAsync(id, cancellationToken);
        return Ok(post);
    }


    [Authorize]
    [HttpPost]
    [SwaggerOperation(Description = "Creates a new post.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreatePost([FromBody] BasicPostDto post, CancellationToken cancellationToken = default)
    {
        await _postsService.CreatePostAsync(new CreatePostRequest(post.Title, post.Content), cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("{id}")]
    [SwaggerOperation(Description = "Edits an existing post that haven't been published.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdatePost(string id, [FromBody] BasicPostDto post, CancellationToken cancellationToken = default)
    {
        await _postsService.EditPostAsync(new EditPostRequest(id, post.Title, post.Content), cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("{id}/publish")]
    [SwaggerOperation(Description = "Publishes a post.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PublishPost(string id, CancellationToken cancellationToken = default)
    {
        await _postsService.PublishPostAsync(id, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerOperation(Description = "Deletes a post.")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeletePost(string id, CancellationToken cancellationToken = default)
    {
        await _postsService.DeletePostAsync(id, cancellationToken);
        return Ok();
    }
}