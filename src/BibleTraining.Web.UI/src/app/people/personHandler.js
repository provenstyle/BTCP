new function(){

    bt.package(this, {
        name:    "person",
        imports: "miruken.callback,miruken.mvc,bt,serviceBus",
        exports: "PersonHandler"
    });

    eval(this.imports);

    const Result = Model.extend();

    const PersonResult = Result.extend({
        "$type": "BibleTraining.Api.Person.PersonResult, BibleTraining.Api",
        $properties: {
            people: { map: Person }
        }
    });

    const GetPeople = Request(PersonResult).extend({
        $properties: {
            "$type": "BibleTraining.Api.Person.GetPeople, BibleTraining.Api",
            ids:           undefined,
            keyProperties: undefined
        }
    });

    const CreatePerson = Request(Person).extend({
        $properties: {
            "$type": "BibleTraining.Api.Person.CreatePerson, BibleTraining.Api",
            resource: undefined
        }
    });

    const UpdatePerson = Request(Person).extend({
        $properties: {
            "$type": "BibleTraining.Api.Person.UpdatePerson, BibleTraining.Api",
            resource: undefined
        }
    });

    const RemovePerson = Request(Person).extend({
        $properties: {
            "$type": "BibleTraining.Api.Person.RemovePerson, BibleTraining.Api",
            resource: undefined
        }
    });

    const PersonHandler = CallbackHandler.extend(PersonFeature, {
        people() {
            return ServiceBus($composer).process(new GetPeople()).then(data => {
                return data.people;
            });
        },
        person(id) {
            return ServiceBus($composer).process(new GetPeople({ids: [id]})).then(data => {
                return (data.people && data.people.length > 0)
                    ? data.people[0]
                    : undefined;
            });
        },
        createPerson(person) {
            const config = $composer.resolve(Configuration);
            person.createdBy = person.modifiedBy = config.userName;
            return ServiceBus($composer).process(new CreatePerson({ resource: person })).then(data => {
                return person.fromData(data);
            });
        },
        updatePerson(person) {
            const config = $composer.resolve(Configuration);
            person.modifiedBy = config.userName;
            return ServiceBus($composer).process(new UpdatePerson({ resource: person })).then(data => {
                return person.fromData(data);
            });
        },
        removePerson(person) {
            const config = $composer.resolve(Configuration);
            person.modifiedBy = config.userName;
            return ServiceBus($composer).process(new RemovePerson({ resource: person })).then(data => {
                return person.fromData(data);
            });
        }
    });

    eval(this.exports);

};
