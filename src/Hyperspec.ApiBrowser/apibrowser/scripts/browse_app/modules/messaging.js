////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012 Mindspace, LLC - http://www.gridlinked.info
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// @author Thomas Burleson
//
////////////////////////////////////////////////////////////////////////////////


!(function ($) {

    // ************************************************************************************
    // Define AngularJS Module 'messaging':
    // Prepares publish/subscribe mechanisms for AngularJS apps
    //
    // NOTE: Requires jQuery 1.7.x
    //
    // ************************************************************************************

    //
    // This is the standard publish and subscribe feature!
    //
    var globalRegistry = {},
		getInstance = function (id) {
		    var queue;
		    var dispatcher = id && globalRegistry[id];

		    if (!dispatcher) {
		        // Build publish/subscribe feature using jQuery v1.7.x Callbacks class
		        queue = $.Callbacks();

		        dispatcher = {
		            publish: queue.fire,
		            subscribe: queue.add,
		            unsubscribe: queue.remove
		        };
		        if (id)
		            globalRegistry[id] = dispatcher;
		    }

		    return dispatcher;
		};

    /**
	 * Use AngularJS and jQuery Callbacks to create publish/subscribe mechanism that can be used
	 * as a shared service across controllers, delegates, and other entities.
	 *
	 * See http://webcache.googleusercontent.com/search?q=cache:EPgxq15YksAJ:api.jquery.com/jQuery.Callbacks
	 */
    angular.module("messaging", [])
		.service('MessageQueue', function MessageQueue() {
		    // Return API wth one (1) method: getInstance()
		    return {
		        /**
                 * Factory method that builds a unique queue instance;
                 * based on a
                 *
                 * @param id  String which is specific ID or topic; is cached for application usage
                 * @return Object custom queue (for ID) with publish(),subscribe(), and unsubscribe() methods
                 */
		        getInstance: getInstance
		    };
		})
		.factory("Pipeline", ["MessageQueue", function (MessageQueue) {
		    /**
			 * This is a variation of the Pub/Sub feature to `lock` topics with specific methods
			 * e.g.
			 *
			 *  var api = Pipeline.getInstance("Channel1 Channel2");
			 *
			 *      api.publishChannel1     ( <data> );
			 *      api.subscribeChannel1   ( function(){... } );
			 *      api.unsubscribeChannel1 ( function(){... } );
			 *
			 *      api.publishChannel2     ( <data> );
			 *      api.subscribeChannel2   ( function(){... } );
			 *      api.unsubscribeChannel2 ( function(){... } );
			 */
		    var buildQueueFor = function (actions) {
		        var api = {};

		        angular.forEach(
                    actions.split(' '),
                    function (action) {
                        var customMq = MessageQueue.getInstance(action),

                            publish = "publish" + action,
                            subscribe = "subscribe" + action,
                            unsubscribe = "unSubscribe" + action;

                        // create aliases to standard pub/sub methods

                        api[publish] = customMq.publish;
                        api[subscribe] = customMq.subscribe;
                        api[unsubscribe] = customMq.unsubscribe;
                    }
                );

		        return api;
		    };

		    // Return API wth one (1) method: getInstance()
		    return {
		        /**
				 * Factory method that builds a unique queue instance;
				 * based on a 1 or more actions/topics. Different from the MessageQueue.getInstance()
				 * this factory supports 1..n IDs (defined in a string with space-delimited values)
				 *
				 * @param String which contains 1...n IDs; space-delimited
				 * @return Object custom queue with publish(),subscribe(), and unsubscribe() methods for each topic in
				 */
		        getInstance: buildQueueFor
		    };
		}])
		.factory("EventDispatcher", ["MessageQueue", function (MessageQueue) {
		    var buildDispatcherFor = function (action) {
		        var customMq = MessageQueue.getInstance(action);

		        // create aliases to standard pub/sub methods
		        return {
		            dispatch: customMq.publish,
		            addListener: customMq.subscribe,
		            removeListener: customMq.unsubscribe
		        };
		    };

		    // Return API wth one (1) method: getInstance()
		    return {
		        /**
				 * Factory method that builds a unique Dispatcher instance;
				 * based on 1 event type.
				 *
				 * @param String which contains an eventType
				 * @return Object custom queue with dispatch(),addListener(),
				 *         and removeListener() methods for the event type/topic
				 */
		        getInstance: buildDispatcherFor
		    };
		}]);



    /**
	 *
	 * Define Module 'myApp': AngularJS services for current app (none currently)
	 * NOTE: MyApp depends/requires the 'messaging' module
	 *
	 * The `pipeline` factory will build pub/sub for two channels.
	 * NOTE: this call below will create a map of 6 methods
	 *       {
	 *          publishChannel1     : function( data ) { };
	 *          subscribeChannel1   : function( callbackFunc ){ };
	 *          unsubscribeChannel1 : function( callbackFunc ){ };
	 *
	 *          publishChannel2     : function( data){ };
	 *          subscribeChannel2   : function( callbackFunc ){ };
	 *          unsubscribeChannel2 : function( callbackFunc ){ };
	 *      }
	 *
	 * And the `channel1` factory will build an EventDispatcher messenger
	 *
	 *       {
	 *          dispatch       : function( data ) { };
	 *          addListener    : function( callbackFunc ){ };
	 *          removeListener : function( callbackFunc ){ };
	 *      }
	 */
    /*
    angular.module('PubSubDemo', ['messaging'])
           .factory('pipeline',['Pipeline', function( PipeLine ) {
               return Pipeline.getInstance("Channel1 Channel2");
           }])
           .factory('channel1',['EventDispatcher', function( EventDispatcher ) {
               // Build specific EventDispatchers for topic: Channel1
               return EventDispatcher.getInstance("Channel1");
           }])
           .factory('channel2',['EventDispatcher', function( EventDispatcher ) {
               // Build specific EventDispatchers for topic: Channel1
               return EventDispatcher.getInstance("Channel2");
           }]);;
    */

}).call(window, $);