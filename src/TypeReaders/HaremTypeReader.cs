﻿#pragma warning disable 1591

using Discord.Commands;
using Sanakan.Services.PocketWaifu;
using System;
using System.Threading.Tasks;

namespace Sanakan.TypeReaders
{
    public class HaremTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            switch (input.ToLower())
            {
                case "rarity":
                case "jakość":
                case "jakośc":
                case "jakosc":
                case "jakosć":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HaremType.Rarity));

                case "def":
                case "obrona":
                case "defence":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HaremType.Defence));

                case "atk":
                case "atak":
                case "attack":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HaremType.Attack));

                case "cage":
                case "klatka":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HaremType.Cage));

                case "relacja":
                case "affection":
                    return Task.FromResult(TypeReaderResult.FromSuccess(HaremType.Affection));

                default:
                    return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Nie rozpoznano typu haremu!"));
            }
        }
    }
}