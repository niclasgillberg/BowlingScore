/// <reference path="./typings/jquery/jquery.d.ts"/>

module BowlingScore {
  class Session {
    private currentFrame = 0;
    private currentRoll = 0;
    private frames: Frame[];
    private resultTarget;

    constructor(elems: JQuery, resultTarget: JQuery) {
      var frame: Frame;
      this.resultTarget = resultTarget;
      this.frames = [];

      // Split the inputs for the first nine frames
      for (var i = 0; i < 9; i++) {
        var frameRolls = elems.splice(0, 2);
        frame = new Frame(frameRolls);
        frame.onComplete(this.updateScore.bind(this));
        this.frames.push(frame);
      }

      // Handle the last frame, since it has an extra roll
      frame = new Frame(elems, true);
      frame.onComplete(this.updateScore.bind(this));
      this.frames.push(frame);
    }

    updateScore() {
      var frames = [];
      var continueToNextFrame = true;
      var frameCount = 0;

      // Parse out all complete frames
      while (continueToNextFrame && frameCount < 10) {
        var frame = this.frames[frameCount];
        if (frame.isComplete())
          frames.push(frame.toJson());
        else
          continueToNextFrame = false;

        frameCount++;
      }

      $.post('/score/calculate',{
          frames: frames
      }).then(((data) => {
        this.resultTarget.text(data.score);
      }).bind(this));
    }
  }

  class Frame {
    private listeners: { (): void }[] = [];

    private first: number;
    private second: number;
    private extra: number;
    private includeExtraRoll: boolean;

    constructor(elems: JQuery, extraCounts: boolean = false) {
      console.log('Creating frame');

      // Attach event handlers
      $(elems[0]).on("change", this.updateFirstRoll.bind(this));
      $(elems[1]).on("change", this.updateSecondRoll.bind(this));
      $(elems[2]).on("change", this.updateExtraRoll.bind(this));

      this.includeExtraRoll = extraCounts;
    }

    isComplete() {
      if (this.first == null || this.first == undefined)
        return false;
      if (this.first === 10)
        return true;
      if (this.second == null || this.second == undefined)
        return false;
      if (this.includeExtraRoll && (this.extra == null || this.extra == undefined))
        return false;
      return true;
    }

    private tryComplete() {
      if (!this.isComplete())
        return;
      var listeners = this.listeners;
      for (var l in listeners) {
        if (listeners.hasOwnProperty(l)) {
          var listener = listeners[l];
          listener();
        }
      }
    }

    updateFirstRoll(evt: JQueryEventObject) {
      var newValue = $(evt.currentTarget).val();
      this.first = newValue;

      this.tryComplete();
    }

    updateSecondRoll(evt: JQueryEventObject) {
      var newValue = $(evt.currentTarget).val();
      this.second = newValue;

      this.tryComplete();
    }

    updateExtraRoll(evt: JQueryEventObject) {
      var newValue = $(evt.currentTarget).val();
      this.extra = newValue;

      this.tryComplete();
    }

    onComplete(callback: () => void) {
      this.listeners.push(callback);
    }

    toJson() {
      return {
        first: this.first,
        second: this.second,
        extra: this.extra
      };
    }
  }

  $(() => {
    var session = new Session($('.roll > input'), $('#total-score'));
  });
}