new function() {

    base2.package(this, {
        name:    "bt",
        imports: "miruken,miruken.mvc",
        exports: "toMoment,mapMoment,toMomentTime,mapMomentTime," +
                 "toMomentDuration,mapMomentDuration," +
                 "extract,id,toEntry,idEntry"
    });

    eval(this.imports);

    function toMoment(dateTime) {
        return dateTime && moment(dateTime);
    }

    const mapMoment = Object.freeze({ map: toMoment });

    function toMomentTime(timespan) {
        return timespan && moment(timespan, "HH:mm:ss");
    }

    const mapMomentTime = Object.freeze({ map: toMomentTime });

    function toMomentDuration(duration) {
        return duration && moment.duration(duration);
    }

    const mapMomentDuration = Object.freeze({ map: toMomentDuration });

    function extract(key, map) {
        return function (value, options) {
            value = key(value);
            if (map && $isSomething(value)) {
                value = map(value, options);
            }
            return value;
        };
    }

    function id(map) {
        return extract(v => v.id, map);
    }

    function toEntry(key, map) {
        return function (value, options) {
            if (map) {
                value = map(value, options);
            }
            return [key(value), value];
        };
    }

    function idEntry(map) {
        return toEntry(v => v.id, map);
    }

    eval(this.exports);
};
