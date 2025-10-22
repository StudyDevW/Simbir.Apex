import ApiService from '../../../shared/api/ApiService';
import type { Rule } from '../entity/Rule';

const RulesApiService = new ApiService<Rule>('rules');

export default RulesApiService;