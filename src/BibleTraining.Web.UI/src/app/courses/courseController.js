//import "./playerFeature.js";
//import "./createPlayerController.js";

new function() {

    bt.package(this, {
        name:    "course",
        imports: "miruken.mvc,bt",
        exports: "CourseController"
    });

    eval(this.imports);

    const CourseController = Controller.extend({
        $properties: {
            courses:   []
        },
        initialize() {
            this.base();
            //return PlayerFeature(this.io).players()
            //    .then(players => this.players = players);
        },

        showCourse() {
            return ViewRegion(this.io).show("app/courses/course");
        }
        //goToPlayer(player) {
        //    PlayerController(this.io).next(ctrl => ctrl.showPlayer({ id: player.id }));
        //},
        //create() {
        //    CreatePlayerController(this.io).next(ctrl => ctrl.createPlayer());
        //}
    });

    eval(this.exports);

};
