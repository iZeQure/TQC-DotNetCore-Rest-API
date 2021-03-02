﻿using Discord;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Provides the ability to notifiy.
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Notifies a user as direct message.
        /// </summary>
        /// <param name="discordUser">The user to direct message.</param>
        /// <param name="clanName">The name of the clan.</param>
        Task NotifyUserAsync(IUser discordUser, Enums.ClanNames clanName);

        /// <summary>
        /// Notifies an admin role.
        /// </summary>
        /// <param name="platformId">Used to identify the admin for a platform.</param>
        /// <param name="discordUser">The user which invoked the notification.</param>
        /// <param name="clanName">The name of the clan.</param>
        Task NotifyAdminAsync(byte platformId, IUser discordUser, Enums.ClanNames clanName);
    }
}