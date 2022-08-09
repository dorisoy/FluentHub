namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of UpdateDiscussionComment
    /// </summary>
    public class UpdateDiscussionCommentInput
    {
        /// <summary>
        /// The Node ID of the discussion comment to update.
        /// </summary>
        public ID CommentId { get; set; }

        /// <summary>
        /// The new contents of the comment body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}