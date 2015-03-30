﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

public class Branch : MonoBehaviour {
    public string branchKey;

    public delegate void BranchCallbackWithParams(Dictionary<string, object> parameters, string error);
    public delegate void BranchCallbackWithUrl(string url, string error);
    public delegate void BranchCallbackWithStatus(bool changed, string error);
    public delegate void BranchCallbackWithList(List<string> list, string error);

    #region Public methods

    #region InitSession methods

    public static void initSession() {
        _initSession();
    }

    public static void  initSession(bool isReferrable) {
        _initSessionAsReferrable(isReferrable);
    }

    public static void initSession(BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _initSessionWithCallback(callbackId);
    }

    public static void initSession(bool isReferrable, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _initSessionAsReferrableWithCallback(isReferrable, callbackId);
    }

    #endregion

    #region Session Item methods

    public static Dictionary<string, object> getFirstReferringParams() {
        string firstReferringParamsString = _getFirstReferringParams();

        return MiniJSON.Json.Deserialize(firstReferringParamsString) as Dictionary<string, object>;
    }

    public static Dictionary<string, object> getLatestReferringParams() {
        string latestReferringParamsString = _getLatestReferringParams();

        return MiniJSON.Json.Deserialize(latestReferringParamsString) as Dictionary<string, object>;
    }

    public static void resetUserSession() {
        _resetUserSession();
    }

    public static void setIdentity(string userId) {
        _setIdentity(userId);
    }

    public static void setIdentity(string userId, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _setIdentityWithCallback(userId, callbackId);
    }

    public static void logout() {
        _logout();
    }

    #endregion

    #region Configuration methods

    public static void setRetryInterval(int retryInterval) {
        _setRetryInterval(retryInterval);
    }

    public static void setMaxRetries(int maxRetries) {
        _setMaxRetries(maxRetries);
    }

    public static void setNetworkTimeout(int timeout) {
        _setNetworkTimeout(timeout);
    }

    #endregion

    #region User Action methods

    public static void loadActionCounts(BranchCallbackWithStatus callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _loadActionCountsWithCallback(callbackId);
    }

    public static void userCompletedAction(string action) {
        _userCompletedAction(action);
    }

    public static void userCompletedAction(string action, Dictionary<string, object> state) {
        _userCompletedActionWithState(action, MiniJSON.Json.Serialize(state));
    }

    public static int getTotalCountsForAction(string action) {
        return _getTotalCountsForAction(action);
    }

    public static int getUniqueCountsForAction(string action) {
       return _getUniqueCountsForAction(action);
    }

    #endregion

    #region Credit methods

    public static void loadRewards(BranchCallbackWithStatus callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _loadRewardsWithCallback(callbackId);
    }

    public static int getCredits() {
        return _getCredits();
    }

    public static int getCredits(string bucket) {
        return _getCreditsForBucket(bucket);
    }

    public static void redeemRewards(int count) {
        _redeemRewards(count);
    }

    public static void redeemRewards(int count, string bucket) {
        _redeemRewardsForBucket(count, bucket);
    }

    public static void getCreditHistory(BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryWithCallback(callbackId);
    }

    public static void getCreditHistory(string bucket, BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryForBucketWithCallback(bucket, callbackId);
    }

    public static void getCreditHistory(string creditTransactionId, int length, int order, BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();

        _branchCallbacks[callbackId] = callback;

        _getCreditHistoryForTransactionWithLengthOrderAndCallback(creditTransactionId, length, order, callbackId);
    }

    public static void getCreditHistory(string bucket, string creditTransactionId, int length, int order, BranchCallbackWithList callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(bucket, creditTransactionId, length, order, callbackId);
    }

    #endregion

    #region Content URL Methods

    public static void getContentURL(Dictionary<string, object> parameters, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getContentUrlWithParamsChannelAndCallback(MiniJSON.Json.Serialize(parameters), channel, callbackId);
    }

    public static void getContentURL(Dictionary<string, object> parameters, List<string> tags, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getContentUrlWithParamsTagsChannelAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, callbackId);
    }

    #endregion

    #region Short URL Generation methods

    public static void getShortURL(BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithCallback(callbackId);
    }

    public static void getShortURL(Dictionary<string, object> parameters, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsAndCallback(MiniJSON.Json.Serialize(parameters), callbackId);
    }

    public static void getShortURLWithTags(Dictionary<string, object> parameters, List<string> tags, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsTagsChannelFeatureStageAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, callbackId);
    }

    public static void getShortURLWithTags(Dictionary<string, object> parameters, List<string> tags, string channel, string feature, string stage, string alias, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, alias, callbackId);
    }

    public static void getShortURLWithTags(int type, Dictionary<string, object> parameters, List<string> tags, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, type, callbackId);
    }

    public static void getShortURLWithTags(Dictionary<string, object> parameters, List<string>tags, string channel, string feature, string stage, int matchDuration, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, feature, stage, matchDuration, callbackId);
    }

    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsChannelFeatureAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, callbackId);
    }

    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsChannelFeatureStageAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, callbackId);
    }

    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, string stage, string alias, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsChannelFeatureStageAliasAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, alias, callbackId);
    }

    public static void getShortURL(int type, Dictionary<string, object> parameters, string channel, string feature, string stage, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsChannelFeatureStageTypeAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, type, callbackId);
    }

    public static void getShortURL(Dictionary<string, object> parameters, string channel, string feature, string stage, int matchDuration, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(MiniJSON.Json.Serialize(parameters), channel, feature, stage, matchDuration, callbackId);
    }

    #endregion

    #region Referral Methods

    public static void getReferralURL(Dictionary<string, object> parameters, List<string> tags, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralUrlWithParamsTagsChannelAndCallback(MiniJSON.Json.Serialize(parameters), MiniJSON.Json.Serialize(tags), channel, callbackId);
    }

    public static void getReferralURL(Dictionary<string, object> parameters, string channel, BranchCallbackWithUrl callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralUrlWithParamsChannelAndCallback(MiniJSON.Json.Serialize(parameters), channel, callbackId);
    }

    public static void getReferralCode(BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralCodeWithCallback(callbackId);
    }

    public static void getReferralCode(int amount, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralCodeWithAmountAndCallback(amount, callbackId);
    }

    public static void getReferralCode(string prefix, int amount, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralCodeWithPrefixAmountAndCallback(prefix, amount, callbackId);
    }

    public static void getReferralCode(int amount, DateTime expiration, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralCodeWithAmountExpirationAndCallback(amount, expiration.ToString("yyyy-MM-ddTHH:mm:ssZ"), callbackId);
    }

    public static void getReferralCode(string prefix, int amount, DateTime expiration, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralCodeWithPrefixAmountExpirationAndCallback(prefix, amount, expiration.ToString("yyyy-MM-ddTHH:mm:ssZ"), callbackId);
    }

    public static void getReferralCode(string prefix, int amount, DateTime expiration, string bucket, int calcType, int location, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(prefix, amount, expiration.ToString("yyyy-MM-ddTHH:mm:ssZ"), bucket, calcType, location, callbackId);
    }

    public static void validateReferralCode(string code, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _validateReferralCodeWithCallback(code, callbackId);
    }

    public static void applyReferralCode(string code, BranchCallbackWithParams callback) {
        var callbackId = _getNextCallbackId();
        
        _branchCallbacks[callbackId] = callback;
        
        _applyReferralCodeWithCallback(code, callbackId);
    }

    #endregion

    #endregion

    #region Private methods

    public void Awake() {
        name = "Branch";

        DontDestroyOnLoad(gameObject);

        _setBranchKey(branchKey);
    }

    #region Platform Loading Methods
    
    #if UNITY_IPHONE
    
    [DllImport ("__Internal")]
    private static extern void _setBranchKey(string branchKey);
    
    [DllImport ("__Internal")]
    private static extern void _initSession();
    
    [DllImport ("__Internal")]
    private static extern void _initSessionAsReferrable(bool isReferrable);
    
    [DllImport ("__Internal")]
    private static extern void _initSessionWithCallback(string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _initSessionAsReferrableWithCallback(bool isReferrable, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern string _getFirstReferringParams();
    
    [DllImport ("__Internal")]
    private static extern string _getLatestReferringParams();
    
    [DllImport ("__Internal")]
    private static extern void _resetUserSession();
    
    [DllImport ("__Internal")]
    private static extern void _setIdentity(string userId);
    
    [DllImport ("__Internal")]
    private static extern void _setIdentityWithCallback(string userId, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _logout();
    
    [DllImport ("__Internal")]
    private static extern void _setRetryInterval(int retryInterval);
    
    [DllImport ("__Internal")]
    private static extern void _setMaxRetries(int maxRetries);
    
    [DllImport ("__Internal")]
    private static extern void _setNetworkTimeout(int timeout);
    
    [DllImport ("__Internal")]
    private static extern void _loadActionCountsWithCallback(string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _userCompletedAction(string action);
    
    [DllImport ("__Internal")]
    private static extern void _userCompletedActionWithState(string action, string stateDict);
    
    [DllImport ("__Internal")]
    private static extern int _getTotalCountsForAction(string action);
    
    [DllImport ("__Internal")]
    private static extern int _getUniqueCountsForAction(string action);
    
    [DllImport ("__Internal")]
    private static extern void _loadRewardsWithCallback(string callbackId);
    
    [DllImport ("__Internal")]
    private static extern int _getCredits();
    
    [DllImport ("__Internal")]
    private static extern void _redeemRewards(int count);
    
    [DllImport ("__Internal")]
    private static extern int _getCreditsForBucket(string bucket);
    
    [DllImport ("__Internal")]
    private static extern void _redeemRewardsForBucket(int count, string bucket);
    
    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryWithCallback(string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryForBucketWithCallback(string bucket, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryForTransactionWithLengthOrderAndCallback(string creditTransactionId, int length, int order, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(string bucket, string creditTransactionId, int length, int order, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getContentUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getContentUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithCallback(string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsAndCallback(string parametersDict, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string alias, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int type, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int matchDuration, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureAndCallback(string parametersDict, string channel, string feature, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageAndCallback(string parametersDict, string channel, string feature, string stage, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parametersDict, string channel, string feature, string stage, string alias, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parametersDict, string channel, string feature, string stage, int type, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string channel, string feature, string stage, int matchDuration, string callbackId);

    [DllImport ("__Internal")]
    private static extern void _getReferralUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithCallback(string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithAmountAndCallback(int amount, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _validateReferralCodeWithCallback(string code, string callbackId);
    
    [DllImport ("__Internal")]
    private static extern void _applyReferralCodeWithCallback(string code, string callbackId);
    
    #elif UNITY_ANDROID

    private static void _setBranchKey(string branchKey) {
        BranchAndroidWrapper.setBranchKey(branchKey);
    }

    private static void _initSession() {
        BranchAndroidWrapper.initSession();
    }

    private static void _initSessionAsReferrable(bool isReferrable) {
        BranchAndroidWrapper.initSessionAsReferrable(isReferrable);
    }

    private static void _initSessionWithCallback(string callbackId) {
        BranchAndroidWrapper.initSessionWithCallback(callbackId);
    }

    private static void _initSessionAsReferrableWithCallback(bool isReferrable, string callbackId) {
        BranchAndroidWrapper.initSessionAsReferrableWithCallback(isReferrable, callbackId);
    }

    private static string _getFirstReferringParams() {
        return BranchAndroidWrapper.getFirstReferringParams();
    }
    
    private static string _getLatestReferringParams() {
        return BranchAndroidWrapper.getLatestReferringParams();
    }
    
    private static void _resetUserSession() {
        BranchAndroidWrapper.resetUserSession();
    }
    
    private static void _setIdentity(string userId) {
        BranchAndroidWrapper.setIdentity(userId);
    }
    
    private static void _setIdentityWithCallback(string userId, string callbackId) {
        BranchAndroidWrapper.setIdentityWithCallback(userId, callbackId);
    }
    
    private static void _logout() {
        BranchAndroidWrapper.logout();
    }
    
    private static void _setRetryInterval(int retryInterval) {
        BranchAndroidWrapper.setRetryInterval(retryInterval);
    }
    
    private static void _setMaxRetries(int maxRetries) {
        BranchAndroidWrapper.setMaxRetries(maxRetries);
    }
    
    private static void _setNetworkTimeout(int timeout) {
        BranchAndroidWrapper.setNetworkTimeout(timeout);
    }
    
    private static void _loadActionCountsWithCallback(string callbackId) {
        BranchAndroidWrapper.loadActionCountsWithCallback(callbackId);
    }
    
    private static void _userCompletedAction(string action) {
        BranchAndroidWrapper.userCompletedAction(action);
    }
    
    private static void _userCompletedActionWithState(string action, string stateDict) {
        BranchAndroidWrapper.userCompletedActionWithState(action, stateDict);
    }
    
    private static int _getTotalCountsForAction(string action) {
        return BranchAndroidWrapper.getTotalCountsForAction(action);
    }
    
    private static int _getUniqueCountsForAction(string action) {
        return BranchAndroidWrapper.getUniqueCountsForAction(action);
    }
    
    private static void _loadRewardsWithCallback(string callbackId) {
        BranchAndroidWrapper.loadRewardsWithCallback(callbackId);
    }
    
    private static int _getCredits() {
        return BranchAndroidWrapper.getCredits();
    }

    private static void _redeemRewards(int count) {
        BranchAndroidWrapper.redeemRewards(count);
    }
    
    private static int _getCreditsForBucket(string bucket) {
        return BranchAndroidWrapper.getCreditsForBucket(bucket);
    }

    private static void _redeemRewardsForBucket(int count, string bucket) {
        BranchAndroidWrapper.redeemRewardsForBucket(count, bucket);
    }

    private static void _getCreditHistoryWithCallback(string callbackId) {
        BranchAndroidWrapper.getCreditHistoryWithCallback(callbackId);
    }
    
    private static void _getCreditHistoryForBucketWithCallback(string bucket, string callbackId) {
        BranchAndroidWrapper.getCreditHistoryForBucketWithCallback(bucket, callbackId);
    }

    private static void _getCreditHistoryForTransactionWithLengthOrderAndCallback(string creditTransactionId, int length, int order, string callbackId) {
        BranchAndroidWrapper.getCreditHistoryForTransactionWithLengthOrderAndCallback(creditTransactionId, length, order, callbackId);
    }
    
    private static void _getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(string bucket, string creditTransactionId, int length, int order, string callbackId) {
        BranchAndroidWrapper.getCreditHistoryForBucketWithTransactionLengthOrderAndCallback(bucket, creditTransactionId, length, order, callbackId);
    }
    
    private static void _getContentUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId) {
        BranchAndroidWrapper.getContentUrlWithParamsChannelAndCallback(parametersDict, channel, callbackId);
    }
    
    private static void _getContentUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId) {
        BranchAndroidWrapper.getContentUrlWithParamsTagsChannelAndCallback(parametersDict, tags, channel, callbackId);
    }
    
    private static void _getShortURLWithCallback(string callbackId) {
        BranchAndroidWrapper.getShortURLWithCallback(callbackId);
    }
    
    private static void _getShortURLWithParamsAndCallback(string parametersDict, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsAndCallback(parametersDict, callbackId);
    }
    
    private static void _getShortURLWithParamsTagsChannelFeatureStageAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageAndCallback(parametersDict, tags, channel, feature, stage, callbackId);
    }
    
    private static void _getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(string parametersDict, string tags, string channel, string feature, string stage, string alias, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageAliasAndCallback(parametersDict, tags, channel, feature, stage, alias, callbackId);
    }
    
    private static void _getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int type, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageTypeAndCallback(parametersDict, tags, channel, feature, stage, type, callbackId);
    }
    
    private static void _getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string tags, string channel, string feature, string stage, int matchDuration, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsTagsChannelFeatureStageMatchDurationAndCallback(parametersDict, tags, channel, feature, stage, matchDuration, callbackId);
    }
    
    private static void _getShortURLWithParamsChannelFeatureAndCallback(string parametersDict, string channel, string feature, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureAndCallback(parametersDict, channel, feature, callbackId);
    }
    
    private static void _getShortURLWithParamsChannelFeatureStageAndCallback(string parametersDict, string channel, string feature, string stage, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageAndCallback(parametersDict, channel, feature, stage, callbackId);
    }
    
    private static void _getShortURLWithParamsChannelFeatureStageAliasAndCallback(string parametersDict, string channel, string feature, string stage, string alias, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageAliasAndCallback(parametersDict, channel, feature, stage, alias, callbackId);
    }
    
    private static void _getShortURLWithParamsChannelFeatureStageTypeAndCallback(string parametersDict, string channel, string feature, string stage, int type, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageTypeAndCallback(parametersDict, channel, feature, stage, type, callbackId);
    }
    
    private static void _getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(string parametersDict, string channel, string feature, string stage, int matchDuration, string callbackId) {
        BranchAndroidWrapper.getShortURLWithParamsChannelFeatureStageMatchDurationAndCallback(parametersDict, channel, feature, stage, matchDuration, callbackId);
    }
    
    private static void _getReferralUrlWithParamsTagsChannelAndCallback(string parametersDict, string tags, string channel, string callbackId) {
        BranchAndroidWrapper.getReferralUrlWithParamsTagsChannelAndCallback(parametersDict, tags, channel, callbackId);
    }
    
    private static void _getReferralUrlWithParamsChannelAndCallback(string parametersDict, string channel, string callbackId) {
        BranchAndroidWrapper.getReferralUrlWithParamsChannelAndCallback(parametersDict, channel, callbackId);
    }
    
    private static void _getReferralCodeWithCallback(string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithCallback(callbackId);
    }
    
    private static void _getReferralCodeWithAmountAndCallback(int amount, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithAmountAndCallback(amount, callbackId);
    }
    
    private static void _getReferralCodeWithPrefixAmountAndCallback(string prefix, int amount, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithPrefixAmountAndCallback(prefix, amount, callbackId);
    }
    
    private static void _getReferralCodeWithAmountExpirationAndCallback(int amount, string expiration, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithAmountExpirationAndCallback(amount, expiration, callbackId);
    }
    
    private static void _getReferralCodeWithPrefixAmountExpirationAndCallback(string prefix, int amount, string expiration, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithPrefixAmountExpirationAndCallback(prefix, amount, expiration, callbackId);
    }
    
    private static void _getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(string prefix, int amount, string expiration, string bucket, int calcType, int location, string callbackId) {
        BranchAndroidWrapper.getReferralCodeWithPrefixAmountExpirationBucketTypeLocationAndCallback(prefix, amount, expiration, bucket, calcType, location, callbackId);
    }
    
    private static void _validateReferralCodeWithCallback(string code, string callbackId) {
        BranchAndroidWrapper.validateReferralCodeWithCallback(code, callbackId);
    }
    
    private static void _applyReferralCodeWithCallback(string code, string callbackId) {
        BranchAndroidWrapper.applyReferralCodeWithCallback(code, callbackId);
    }

    #endif
    
    #endregion

    #region Callback management

    public void _asyncCallbackWithParams(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        var parameters = callbackDict["params"] as Dictionary<string, object>;
        var error = callbackDict["error"] as string;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithParams;
        callback(parameters, error);
    }

    public void _asyncCallbackWithStatus(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        var status = (callbackDict["status"] as bool?).Value || false;
        var error = callbackDict["error"] as string;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithStatus;
        callback(status, error);
    }

    public void _asyncCallbackWithList(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        var list = callbackDict["list"] as List<string>;
        var error = callbackDict["error"] as string;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithList;
        callback(list, error);
    }

    public void _asyncCallbackWithUrl(string callbackDictString) {
        var callbackDict = MiniJSON.Json.Deserialize(callbackDictString) as Dictionary<string, object>;
        var callbackId = callbackDict["callbackId"] as string;
        var url = callbackDict["url"] as string;
        var error = callbackDict["error"] as string;

        var callback = _branchCallbacks[callbackId] as BranchCallbackWithUrl;
        callback(url, error);
    }

    private static string _getNextCallbackId() {
        return "BranchCallbackId" + (++_nextCallbackId);
    }

    #endregion

    #endregion

    private static int _nextCallbackId = 0;
    private static Dictionary<string, object> _branchCallbacks = new Dictionary<string, object>();
}