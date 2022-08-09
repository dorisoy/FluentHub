namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of CreateRef
    /// </summary>
    public class CreateRefInput
    {
        /// <summary>
        /// The Node ID of the Repository to create the Ref in.
        /// </summary>
        public ID RepositoryId { get; set; }

        /// <summary>
        /// The fully qualified name of the new Ref (ie: `refs/heads/my_new_branch`).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The GitObjectID that the new Ref shall target. Must point to a commit.
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}