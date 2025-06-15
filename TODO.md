# TODO List

## Priorities

- [ ] HIGH | Test: Add unit tests for Gobbler and Regex conversion
- [ ] MED | Perf: Make regex check in Gobbler to be inline, saving on memory and time
- [ ] MED | CQBC: Abstract Gobbler to be reusable and extensible
- [ ] LOW | Util: Add configuration options for Gobbler 
- [ ] LOW | Util: Add service setup for Gobbler

## Backlog

- [ ] Util: Add internal logging, printing to console or utilizing an injected Msft logger
- [ ] Util: Standardize namespaces for simplified imports
- [ ] Util: Abstract public options to handle safe-private set and get methods
- [ ] Util: Add internal logging, printing to console or utilizing an injected Msft logger

## Completed

- [x] HIGH | Test: Add integration tests for Gobbler and Regex conversion
- [x] HIGH | Bug: Fix relative path at depth 2-n for regex matching
- [x] HIGH | Bug: Fix regex conversion when using UNC paths