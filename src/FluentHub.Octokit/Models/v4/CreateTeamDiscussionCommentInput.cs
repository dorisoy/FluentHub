namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of CreateTeamDiscussionComment
    /// </summary>
    public class CreateTeamDiscussionCommentInput
    {
        /// <summary>
        /// The ID of the discussion to which the comment belongs.
        /// </summary>
        public ID DiscussionId { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}