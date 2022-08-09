namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of AddLabelsToLabelable
    /// </summary>
    public class AddLabelsToLabelableInput
    {
        /// <summary>
        /// The id of the labelable object to add labels to.
        /// </summary>
        public ID LabelableId { get; set; }

        /// <summary>
        /// The ids of the labels to add.
        /// </summary>
        public List<ID> LabelIds { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}