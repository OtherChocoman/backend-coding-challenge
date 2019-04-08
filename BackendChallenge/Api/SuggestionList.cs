using BackendChallenge.Application;
using System.Collections.Generic;

namespace BackendChallenge.Api
{
    public class SuggestionList
    {
        public IReadOnlyList<Suggestion> Suggestions { get; set; }
    }
}
