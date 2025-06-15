# TODO List

## Priorities

- [ ] HIGH | Test: Add unit tests for Gobbler and Regex conversion
- [ ] HIGH | Bug: Fix regex filter with N depth exclusion and inclusion
- [ ] MED | Perf: Make regex check in Gobbler to be inline, saving on memory and time
- [ ] MED | CQBC: Abstract Gobbler to be reusable and extensible
- [ ] LOW | Util: Add configuration options for Gobbler 
- [ ] LOW | Util: Add service setup for Gobbler

## Backlog

- [ ] Util: Add internal logging, printing to console or utilizing an injected Msft logger

## Completed

- [x] Test: Add integration tests for Gobbler and Regex conversion
- [x] Bug: Fix relative path at depth 2-n for regex matching
- [x] Bug: Fix regex conversion when using UNC paths
- [x] Util: Standardize namespaces for simplified imports
- [x] Util: Abstract public options to handle safe-private set and get methods
- [x] 