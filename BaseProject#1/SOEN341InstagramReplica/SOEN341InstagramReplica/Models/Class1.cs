using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOEN341InstagramReplica.Models;

namespace SOEN341InstagramReplica.Models
{
    public class UserPostsAndComments
    {
        public UserPost currentUserPost { get; set; }
        public List<Comment> currentUserPostComments { get; set; }
    }
}