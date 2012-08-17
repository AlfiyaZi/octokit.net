﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Burr.Helpers;
using Burr.SimpleJson;

namespace Burr
{
    public class GitHubModelMap : IGitHubModelMap
    {
        static readonly Dictionary<Type, Func<JObject, object>> toObjects = new Dictionary<Type, Func<JObject, object>>();
        static readonly Dictionary<Type, Func<object, JObject>> fromObjects = new Dictionary<Type, Func<object, JObject>>();

        static GitHubModelMap()
        {
            toObjects.Add(typeof (User), JObjectToUser);
            toObjects.Add(typeof (Authorization), JObjectToAuthorization);
            toObjects.Add(typeof (IEnumerable<Authorization>), JObjectToAuthorizations);
            toObjects.Add(typeof (Repository), JObjectToRepository);
            toObjects.Add(typeof (List<Repository>), JObjectToRepositories);

            fromObjects.Add(typeof (UserUpdate), x => UserToJObject((UserUpdate)x));
            fromObjects.Add(typeof (AuthorizationUpdate), x => AuthorizationToJObject((AuthorizationUpdate)x));
        }

        public T For<T>(JObject obj)
        {
            Ensure.ArgumentNotNull(obj, "obj");

            return (T)toObjects[typeof (T)](obj);
        }

        public JObject For(object obj)
        {
            Ensure.ArgumentNotNull(obj, "obj");

            return fromObjects[obj.GetType()](obj);
        }

        public static JObject AuthorizationToJObject(AuthorizationUpdate auth)
        {
            var dict = new Dictionary<string, JObject>();

            if (auth.Scopes != null)
                dict.Add("scopes", JObject.CreateArray(auth.Scopes.Select(JObject.CreateString).ToList()));
            if (auth.Note != null)
                dict.Add("note", JObject.CreateString(auth.Note));
            if (auth.NoteUrl != null)
                dict.Add("note_url", JObject.CreateString(auth.NoteUrl));

            return JObject.CreateObject(dict);
        }

        public static IEnumerable<Authorization> JObjectToAuthorizations(JObject jObj)
        {
            return jObj.ArrayValue.Select(JObjectToAuthorization);
        }

        public static Authorization JObjectToAuthorization(JObject jObj)
        {
            var auth = new Authorization
                       {
                           Application = new Application
                                         {
                                             Name = (string)jObj["app"]["name"],
                                             Url = (string)jObj["app"]["url"],
                                         },
                           CreatedAt = DateTimeOffset.Parse((string)jObj["created_at"], CultureInfo.InvariantCulture),
                           Id = (int)jObj["id"],
                           Note = (string)jObj["note"],
                           NoteUrl = (string)jObj["note_url"],
                           Token = (string)jObj["token"],
                           UpdateAt = DateTimeOffset.Parse((string)jObj["updated_at"], CultureInfo.InvariantCulture),
                           Url = (string)jObj["url"],
                       };

            var scopes = jObj["scopes"].ArrayValue;
            if (scopes != null)
                auth.Scopes = scopes.Select(x => x.StringValue).ToArray();

            return auth;
        }

        public static JObject UserToJObject(UserUpdate user)
        {
            var dict = new Dictionary<string, JObject>();

            if (user.Name != null)
                dict.Add("name", JObject.CreateString(user.Name));
            if (user.Email != null)
                dict.Add("email", JObject.CreateString(user.Email));
            if (user.Blog != null)
                dict.Add("blog", JObject.CreateString(user.Blog));
            if (user.Company != null)
                dict.Add("company", JObject.CreateString(user.Company));
            if (user.Location != null)
                dict.Add("location", JObject.CreateString(user.Location));
            if (user.Bio != null)
                dict.Add("bio", JObject.CreateString(user.Bio));
            if (user.Hireable.HasValue)
                dict.Add("hireable", JObject.CreateBoolean(user.Hireable.Value));

            return JObject.CreateObject(dict);
        }

        public static User JObjectToUser(JObject jObj)
        {
            var user = new User
                       {
                           Followers = (int)jObj["followers"],
                           Type = (string)jObj["type"],
                           Hireable = (bool)jObj["hireable"],
                           AvatarUrl = (string)jObj["avatar_url"],
                           Bio = (string)jObj["bio"],
                           HtmlUrl = (string)jObj["html_url"],
                           CreatedAt = DateTimeOffset.Parse((string)jObj["created_at"], CultureInfo.InvariantCulture),
                           PublicRepos = (int)jObj["public_repos"],
                           Blog = (string)jObj["blog"],
                           Url = (string)jObj["url"],
                           PublicGists = (int)jObj["public_gists"],
                           Following = (int)jObj["following"],
                           Company = (string)jObj["company"],
                           Name = (string)jObj["name"],
                           Location = (string)jObj["location"],
                           Id = (long)jObj["id"],
                           Email = (string)jObj["email"],
                           Login = (string)jObj["login"],
                       };

            if (jObj.ObjectValue.ContainsKey("plan"))
            {
                user.Plan = new Plan
                            {
                                Collaborators = (int)jObj["plan"]["collaborators"],
                                Name = (string)jObj["plan"]["name"],
                                Space = (int)jObj["plan"]["space"],
                                PrivateRepos = (int)jObj["plan"]["private_repos"],
                            };
            }

            return user;
        }

        public static List<Repository> JObjectToRepositories(JObject jObj)
        {
            return jObj.ArrayValue.Select(JObjectToRepository).ToList();
        }

        public static Repository JObjectToRepository(JObject jObj)
        {
            var repo = new Repository
            {
                CloneUrl = (string)jObj["clone_url"],
                OpenIssuesCount = (int)jObj["open_issues_count"],
                HasIssues = (bool)jObj["has_issues"],
                WatchersCount = (int)jObj["watchers_count"],
                PushedAt = DateTimeOffset.Parse((string)jObj["pushed_at"], CultureInfo.InvariantCulture),
                ForksCount = (int)jObj["forks_count"],
                Language = (string)jObj["language"],
                FullName = (string)jObj["full_name"],
                MasterBranch = (string)jObj["master_branch"],
                SshUrl = (string)jObj["ssh_url"],
                MirrorUrl = (string)jObj["mirror_url"],
                HasDownloads = (bool)jObj["has_downloads"],
                Homepage = (string)jObj["homepage"],
                Size = (long)jObj["size"],
                IsFork = (bool)jObj["fork"],
                SvnUrl = (string)jObj["svn_url"],
                CreatedAt = DateTimeOffset.Parse((string)jObj["created_at"], CultureInfo.InvariantCulture),
                Description = (string)jObj["description"],
                Name = (string)jObj["name"],
                Url = (string)jObj["url"],
                NetworkCount = (int)jObj["network_count"],
                HasWiki = (bool)jObj["has_wiki"],
                HtmlUrl = (string)jObj["html_url"],
                IsPrivate = (bool)jObj["private"],
                Id = (long)jObj["id"],
                GitUrl = (string)jObj["git_url"],
                UpdatedAt = DateTimeOffset.Parse((string)jObj["updated_at"], CultureInfo.InvariantCulture),
            };

            if(jObj.ObjectValue.ContainsKey("owner"))
            {
                repo.Owner = new User
                             {
                                 Login = (string)jObj["owner"]["login"],
                                 Url = (string)jObj["owner"]["url"],
                                 Id = (long)jObj["owner"]["id"],
                                 AvatarUrl = (string)jObj["owner"]["avatar_url"],
                             };
            }

            return repo;
        }
    }
}