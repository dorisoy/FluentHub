namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of DeleteDiscussion
    /// </summary>
    public class DeleteDiscussionInput
    {
        /// <summary>
        /// The id of the discussion to delete.
        /// </summary>
        public ID Id { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}