new function () {

    base2.package(this, {
        name:    "bt",
        imports: "miruken",
        exports: "query,ofType,isType,or,not"
    });

    eval(this.imports);

    const NO_RESULTS = Object.freeze([]);

    const query = Base.extend({
        constructor(from) {
            from = $isNothing(from) ? NO_RESULTS : Array.from(from);
            this.extend({
                toArray() {
                    return $isPromise(from)
                         ? from.then(f => this.filter(f))
                         : this.filter(from);
                },
                first() {
                    return $isPromise(from)
                         ? from.then(f => this.filter(f)[0])
                         : this.filter(from)[0];
                },
                any(predicate) {
                    return $isPromise(from)
                         ? from.then(f => this.filter(f).some(predicate || True))
                         : this.filter(from).some(predicate || True);
                },
                all(predicate) {
                    return $isPromise(from)
                         ? from.then(f => !predicate || this.filter(from).every(predicate))
                         : !predicate || this.filter(from).every(predicate);
                },
                count(predicate) {
                    return $isPromise(from)
                         ? from.then(f => _count(this.filter(f), predicate))
                         : _count(this.filter(from), predicate);
                }
            });
        },
        filter(source) {
             return source;
        },
        where(...conditions) {
            if (conditions.length === 1 && Array.isArray(conditions[0])) {
                conditions = conditions[0];
            }
            return conditions.length > 0
                 ? $decorate(this, {
                       filter(source) {
                           return this.base(source).filter(item => item &&
                               conditions.every(c => c(item)));
                       }
                   })
                 : this;
        },
        select(selector) {
            return $isFunction(selector)
                 ? $decorate(this, {
                       filter(source) {
                           return Array.from(this.base(source), selector);
                       }
                    })
                 : this;
        },
        selectMany(selector) {
            return $isFunction(selector)
                 ? $decorate(this, {
                     filter(source) {
                         return Array.from(this.base(source), selector)
                            .reduce((a, b) => a.concat(b), []);
                     }
                 })
                 : this;
        },
        update(action) {
            return $isFunction(action)
                 ? $decorate(this, {
                     filter(source) {
                         const items = this.base(source);
                         Array.forEach(items, action);
                         return items;
                     }
                 })
                 : this;
        },
        orderBy(comparer) {
            return $isFunction(comparer)
                 ? $decorate(this, {
                     filter(source) {
                         return this.base(source).sort(comparer);
                     }
                 })
                 : this;
        },
        groupBy(keySelector) {
            return $isFunction(keySelector)
                ? $decorate(this, {
                    filter(source) {
                        const groups = new Map(),
                              items  = this.base(source);
                        items.forEach(item => {
                            const key = keySelector(item);
                            let group = groups.get(key);
                            if (!group) {
                                group = [];
                                groups.set(key, group);
                            }
                            group.push(item);
                        });
                        return [...groups.entries()].map(([key, group]) => {
                            return {
                                key:    key,
                                values: group
                            };
                        });
                    }
                })
                : this;
        },
        distinct() {
            return $decorate(this, {
                filter(source) {
                    return [...new Set(this.base(source))];
                }
            });
        },
        slice(skip, take) {
            return $decorate(this, {
                filter(source) {
                    return this.base(source).slice(skip, skip + take);
                }
            });
        },
        page(page, pageSize) {
            return this.slice((page - 1) * pageSize, pageSize);
        },
        skip(count) {
            return $decorate(this, {
                filter(source) {
                    return this.base(source).slice(count);
                }
            });
        },
        take(count) {
            return $decorate(this, {
                filter(source) {
                    return this.base(source).slice(0, count);
                }
            });
        }
    }, {
        coerce() { return this.new.apply(this, arguments); }
    });

    function _count(items, predicate) {
        return $isFunction(predicate)
             ? items.reduce((count, item) => predicate(item) ? count + 1 : count, 0)
             : items.length;
    }

    function ofType(type) {
        return function(item) {
            return item instanceof type;
        };
    }

    function isType(type) {
        return function(item) {
            return item && (item.constructor === type);
        };
    }

    function or(...conditions) {
        if (conditions.length === 1 && Array.isArray(conditions[0])) {
            conditions = conditions[0];
        }
        return function() {
            for (let condition of conditions) {
                if ($isFunction(condition) && condition.apply(this, arguments)) {
                    return true;
                }
            }
            return false;
        };
    }

    function not(fn) {
        return function () {
            return !fn.apply(this, arguments);
        };
    }

    eval(this.exports);

};
