using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using NttBank.Agent.Agent.Abstractions;

namespace NttBank.Agent.Infrastructure.Conversation;

[ExcludeFromCodeCoverage]
public sealed class HttpConversationContext(
    IHttpContextAccessor httpContextAccessor)
    : IConversationContext
{
    private const string ConversationHeader = "X-Conversation-Id";
    private const string CustomerHeader = "X-Customer-Id";
    private const string ConversationIdItem = "ConversationId";
    private const string CustomerIdItem = "CustomerId";

    public string GetConversationId()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return Guid.NewGuid().ToString("N");
        }

        if (httpContext.Items.TryGetValue(ConversationIdItem, out var itemValue)
            && itemValue is string itemId
            && !string.IsNullOrWhiteSpace(itemId))
        {
            return itemId;
        }

        var headerValue = httpContext.Request.Headers[ConversationHeader].ToString();

        if (!string.IsNullOrWhiteSpace(headerValue))
        {
            return headerValue;
        }

        var generated = Guid.NewGuid().ToString("N");

        httpContext.Items[ConversationIdItem] = generated;

        return generated;
    }

    public string? GetCustomerId()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return null;
        }

        if (httpContext.Items.TryGetValue(CustomerIdItem, out var itemValue)
            && itemValue is string itemId
            && !string.IsNullOrWhiteSpace(itemId))
        {
            return itemId;
        }

        var headerValue = httpContext.Request.Headers[CustomerHeader].ToString();

        return string.IsNullOrWhiteSpace(headerValue) ? null : headerValue;
    }
}
