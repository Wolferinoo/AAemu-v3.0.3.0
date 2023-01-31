﻿namespace AAEmu.Game.Models.Game.Features
{
    public enum Feature
    {
        //Don't mess with these values, ask the core team for questions.
        //Refer to FeatureSet.GetIndexes to interpret
        siege = 0,
        allowFamilyChanges = 1,
        flag_0_2 = 2,
        houseSale = 3, // Allows users to sell their house
        premium = 4,
        flag_0_5 = 5,
        nations = 6,
        flag_0_7 = 7,
        // 8 -> 15 is MaxPlayerLevel
        flag_2_0 = 16,
        flag_2_1 = 17,
        flag_2_2 = 18,
        flag_2_3 = 19,
        flag_2_4 = 20,
        flag_2_5 = 21,
        flag_2_6 = 22,
        flag_2_7 = 23,
        flag_3_0 = 24,
        flag_3_1 = 25,
        flag_3_2 = 26,
        flag_3_3 = 27,
        flag_3_4 = 28,
        flag_3_5 = 29,
        flag_3_6 = 30,
        flag_3_7 = 31,
        flag_4_0 = 32,
        flag_4_1 = 33,
        flag_4_2 = 34,
        flag_4_3 = 35,
        ranking = 36,
        flag_4_5 = 37,
        ingamecashshop = 38,
        customUiButton = 39,
        customsaveload = 40,
        flag_5_1 = 41,
        flag_5_2 = 42,
        bm_mileage = 43,
        aaPoint = 44,
        itemSecure = 45,
        secondpass = 46,
        flag_5_7 = 47,
        slave_customize = 48,
        flag_6_1 = 49,
        flag_6_2 = 50,
        flag_6_3 = 51,
        beautyshopBypass = 52,
        freeLpRaise = 53, //Might not exist for 0.5
        flag_6_6 = 54,
        ingameshopSecondpass = 55,
        backpackProfitShare = 56,
        flag_7_1 = 57,
        sensitiveOpeartion = 58,
        taxItem = 59, // House tax is payed using item instead of gold
        flag_7_4 = 60,
        flag_7_5 = 61,
        flag_7_6 = 62,
        achievement = 63,
        // 64 -> 71 is MateMaxLevel
        flag_9_0 = 72,
        flag_9_1 = 73,
        dwarfWarborn = 74,
        mailCoolTime = 75,
        flag_9_4 = 76,
        flag_9_5 = 77,
        flag_9_6 = 78,
        flag_9_7 = 79,
        flag_10_0_DO_NOT_USE = 80, // setting this flag makes the client crash when connecting
        //These are different positions in 0.5 for some reason..
//        hudAuctionButton = 81,
        flag_81 = 81,
        flag_10_2 = 82,
//        auctionPostBuff = 83,
        flag_83 = 83,
//        houseTaxPrepay = 84,
        flag_84 = 84,
        flag_10_5 = 85,
        flag_10_6 = 86,
        flag_10_7 = 87,

        // 3503 Enum Only
        hudAuctionButton = 89,
        auctionPostBuff = 91,
        itemRepairInBag = 92,
        petOnlyEnchantStone = 93,
        questNpcTag = 94,
        houseTaxPrepay = 95,
        hudBattleFieldButton = 98,
        hudMailBoxButton = 99,
        fastQuestChatBubble = 100,
        todayAssignment = 101,
        forbidTransferChar = 102,
        targetEquipmentWnd = 103,
        indunPortal = 106,
        indunDailyLimit = 110,
        rebuildHouse = 111,
        useTGOS = 112,
        forcePopupTGOS = 113,
        newNameTag = 114,
        reportSpammer = 115,
        hero = 116,
        marketPrice = 117,
        buyPremiuminSelChar = 119,
        expeditionWar = 138,
        freeResurrectionInPlace = 139,
        expeditionLevel = 141,
        itemEvolving = 142,
        useSavedAbilities = 143,
        rankingRenewal = 144,
        hudAuctionBuff = 145,
        accountAttendance = 146,
        expeditionRecruit = 147,
        uiAvi = 148,
        shopOnUI = 149,
        itemLookConvertInBag = 150,
        newReportBaduser = 151,
        expeditionSummon = 153,
        heroBonus = 154,
        hairTwoTone = 158,
        hudBattleFieldBuff = 159,
        expeditionRank = 160,
        mateTypeSummon = 161,
        permissionZone = 162,
        lootGacha = 163,
        itemEvolvingReRoll = 164,
        ranking_myworld_only = 166,
        eloRating = 167,
        nationMemberLimit = 168,
        packageDemolish = 170,
        itemGuide = 172,
        restrictFollow = 173,
        socketExtract = 174,
        itemlookExtract = 175,
        useCharacterListPage = 176,
        renameExpeditionByItem = 177,
        expeditionImmigration = 178,
        highAbility = 179,
        eventWebLink = 180,
        blessuthstin = 181,
        vehicleZoneSimulation = 182,
        itemSmelting = 183,
        characterInfoLivingPoint = 192,
        useForceAttack = 193,
        specialtyTradeGoods = 194,
        reportBadWordUser = 195,
        residentweblink = 197
    }
}
