/// <reference path="./typings/jquery/jquery.d.ts"/>
var BowlingScore;
(function (BowlingScore) {
    var Session = (function () {
        function Session(elems, resultTarget) {
            var frame;
            this.resultTarget = resultTarget;
            this.frames = [];
            for (var i = 0; i < 9; i++) {
                var frameRolls = elems.splice(0, 2);
                frame = new Frame(frameRolls);
                frame.onComplete(this.updateScore.bind(this));
                this.frames.push(frame);
            }
            frame = new Frame(elems, true);
            frame.onComplete(this.updateScore.bind(this));
            this.frames.push(frame);
        }
        Session.prototype.start = function () {
            this.currentFrame = 0;
            this.currentRoll = 0;
        };
        Session.prototype.updateScore = function () {
            var _this = this;
            var frames = [];
            var continueToNextFrame = true;
            var frameCount = 0;
            while (continueToNextFrame && frameCount < 10) {
                var frame = this.frames[frameCount];
                if (frame.isComplete())
                    frames.push(frame.toJson());
                else
                    continueToNextFrame = false;
                frameCount++;
            }
            $.post('/score/calculate', {
                frames: frames
            }).then((function (data) {
                _this.resultTarget.text(data.score);
            }).bind(this));
        };
        return Session;
    })();
    var Frame = (function () {
        function Frame(elems, extraCounts) {
            if (extraCounts === void 0) { extraCounts = false; }
            this.listeners = [];
            console.log('Creating frame');
            $(elems[0]).on("change", this.updateFirstRoll.bind(this));
            $(elems[1]).on("change", this.updateSecondRoll.bind(this));
            $(elems[2]).on("change", this.updateExtraRoll.bind(this));
            this.includeExtraRoll = extraCounts;
        }
        Frame.prototype.isComplete = function () {
            if (this.first == null || this.first == undefined)
                return false;
            if (this.first === 10)
                return true;
            if (this.second == null || this.second == undefined)
                return false;
            if (this.includeExtraRoll && (this.extra == null || this.extra == undefined))
                return false;
            return true;
        };
        Frame.prototype.tryComplete = function () {
            if (!this.isComplete())
                return;
            var listeners = this.listeners;
            for (var l in listeners) {
                if (listeners.hasOwnProperty(l)) {
                    var listener = listeners[l];
                    listener();
                }
            }
        };
        Frame.prototype.updateFirstRoll = function (evt) {
            var newValue = $(evt.currentTarget).val();
            this.first = newValue;
            this.tryComplete();
        };
        Frame.prototype.updateSecondRoll = function (evt) {
            var newValue = $(evt.currentTarget).val();
            this.second = newValue;
            this.tryComplete();
        };
        Frame.prototype.updateExtraRoll = function (evt) {
            var newValue = $(evt.currentTarget).val();
            this.extra = newValue;
            this.tryComplete();
        };
        Frame.prototype.onComplete = function (callback) {
            this.listeners.push(callback);
        };
        Frame.prototype.toJson = function () {
            return {
                first: this.first,
                second: this.second,
                extra: this.extra
            };
        };
        return Frame;
    })();
    $(function () {
        console.log("Application started");
        var session = new Session($('.roll > input'), $('#total-score'));
    });
})(BowlingScore || (BowlingScore = {}));
//# sourceMappingURL=BowlingScore.js.map