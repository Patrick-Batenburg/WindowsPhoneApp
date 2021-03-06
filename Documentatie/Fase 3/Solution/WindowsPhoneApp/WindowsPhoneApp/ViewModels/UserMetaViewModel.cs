﻿using System;
using System.Linq;
using Windows.UI.Xaml;
using Medex.Models;
using System.Collections.ObjectModel;

namespace Medex.ViewModels
{
    public class UserMetaViewModel : ViewModelBase
    {
        private int userId = 0;
        private int taskId = 0;
        private App app = (Application.Current as App);

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public UserMetaViewModel()
        {

        }

        /// <summary>
        /// Retrieve the specific user meta.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="taskId">ID of the task.</param>
        /// <returns>Returns all information about the user meta.</returns>
        public UserMeta GetUserMeta(int taskId)
        {
            UserMeta userMeta = new UserMeta();

            using (var db = new SQLite.SQLiteConnection(app.DB_PATH))
            {
                var _userMeta = (from u in db.Table<UserMeta>()
                                 where u.UserId == app.CURRENT_USER_ID && u.TaskId == taskId
                                 select u).Single();

                userMeta.UserId = _userMeta.UserId;
                userMeta.TaskId = _userMeta.TaskId;
            }
            return userMeta;
        }

        /// <summary>
        /// Retrieve all user meta.
        /// </summary>
        /// <returns>Returns all information about the user meta.</returns>
        public ObservableCollection<UserMetaViewModel> GetUserMetas()
        {
            ObservableCollection<UserMetaViewModel> userMetas = new ObservableCollection<UserMetaViewModel>();

            using (var db = new SQLite.SQLiteConnection(app.DB_PATH))
            {
                var query = (from u in db.Table<UserMeta>()
                             where u.UserId == app.CURRENT_USER_ID
                             select u).ToList();

                foreach (var _user in query)
                {
                    UserMetaViewModel userMeta = new UserMetaViewModel()
                    {
                        TaskId = _user.TaskId,
                        UserId = _user.UserId
                    };
                    userMetas.Add(userMeta);
                }
            }
            return userMetas;
        }

        /// <summary>
        /// Adds a user meta to the database.
        /// </summary>
        /// <param name="userMeta">Helds all information to be inserted into database.</param>
        /// <returns>Returns true on success and false on failure.</returns>
        public bool AddUserMeta(UserMeta userMeta)
        {
            bool result = false;

            using (var db = new SQLite.SQLiteConnection(app.DB_PATH))
            {
                try
                {
                    var existingUserMeta = (from u in db.Table<UserMeta>()
                                            where u.UserId == app.CURRENT_USER_ID && u.TaskId == userMeta.TaskId
                                            select u).SingleOrDefault();

                    if (existingUserMeta == null)
                    {
                        int success = db.Insert(userMeta);
                    }
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Updates existing user meta.
        /// </summary>
        /// <param name="userMeta">Helds all information to update user.</param>
        /// <returns>Returns true on success and false on failure.</returns>
        public bool UpdateUserMeta(UserMetaViewModel userMeta)
        {
            bool result = false;

            using (var db = new SQLite.SQLiteConnection(app.DB_PATH))
            {
                try
                {
                    var existingUserMeta = (from u in db.Table<Models.UserMeta>()
                                            where u.UserId == app.CURRENT_USER_ID && u.TaskId == userMeta.TaskId
                                            select u).SingleOrDefault();

                    if (existingUserMeta != null)
                    {
                        UserId = userMeta.UserId;
                        TaskId = userMeta.TaskId;
                        int success = db.Update(existingUserMeta);
                    }
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes a user meta from the database.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="taskId">ID of the task.</param>
        /// <returns>Returns true on success and false on failure.</returns>
        public bool DeleteUserMeta(int taskId)
        {
            bool result = false;

            using (var db = new SQLite.SQLiteConnection(app.DB_PATH))
            {
                var existingUserMeta = (from u in db.Table<UserMeta>()
                                        where u.UserId == app.CURRENT_USER_ID && u.TaskId == taskId
                                        select u).Single();

                if (db.Delete(existingUserMeta) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                if (userId == value)
                {
                    return;
                }

                userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        public int TaskId
        {
            get
            {
                return taskId;
            }

            set
            {
                if (taskId == value)
                {
                    return;
                }

                taskId = value;
                RaisePropertyChanged("TaskId");
            }
        }
    }
}
