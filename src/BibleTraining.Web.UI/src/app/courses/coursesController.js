//import "./playerFeature.js";
//import "./createPlayerController.js";

new function() {

    bt.package(this, {
        name:    "course",
        imports: "miruken.mvc,bt",
        exports: "CoursesController"
    });

    eval(this.imports);

    const CoursesController = Controller.extend({
        $properties: {
            courses:   []
        },
        initialize() {
            this.base();
            //return PlayerFeature(this.io).players()
            //    .then(players => this.players = players);
        },

        showCourses() {
            return ViewRegion(this.io).show("app/courses/courses");
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
