/**
 * 家族接口
 */
export interface Family {
  id: string
  name: string
  description?: string
  originPlace?: string
  graphLayout?: string
  createdAt: string
  updatedAt: string
}

/**
 * 创建家族请求
 */
export interface CreateFamilyRequest {
  name: string
  description?: string
  originPlace?: string
}

/**
 * 更新家族请求
 */
export interface UpdateFamilyRequest {
  name: string
  description?: string
  originPlace?: string
  graphLayout?: string
}

/**
 * 家族成员接口
 */
export interface FamilyMember {
  id: string
  familyId: string
  name: string
  gender: 'Male' | 'Female'
  birthDate?: string
  deathDate?: string
  fatherId?: string
  motherId?: string
  spouseId?: string
  generation: number
  avatarUrl?: string
  customAttributes?: string
  nodeStyle?: string
  notes?: string
  positionX?: number
  positionY?: number
  createdAt: string
  updatedAt: string
}

/**
 * 创建家族成员请求
 */
export interface CreateFamilyMemberRequest {
  familyId: string
  name: string
  gender: 'Male' | 'Female'
  birthDate?: string
  deathDate?: string
  fatherId?: string
  motherId?: string
  spouseId?: string
  generation: number
  avatarUrl?: string
  customAttributes?: string
  nodeStyle?: string
  notes?: string
  positionX?: number
  positionY?: number
}

/**
 * 更新家族成员请求
 */
export interface UpdateFamilyMemberRequest {
  name: string
  gender: 'Male' | 'Female'
  birthDate?: string
  deathDate?: string
  fatherId?: string
  motherId?: string
  spouseId?: string
  generation: number
  avatarUrl?: string
  customAttributes?: string
  nodeStyle?: string
  notes?: string
  positionX?: number
  positionY?: number
}

/**
 * 成员位置更新
 */
export interface MemberPosition {
  id: string
  x: number
  y: number
}

/**
 * 节点样式配置
 */
export interface NodeStyleConfig {
  fill?: string
  stroke?: string
  lineWidth?: number
  opacity?: number
}

/**
 * G6图节点数据
 */
export interface G6NodeData {
  id: string
  label: string
  member: FamilyMember
  x?: number
  y?: number
  style?: any
}

/**
 * G6图边数据
 */
export interface G6EdgeData {
  id: string
  source: string
  target: string
  type?: string
  style?: any
}

/**
 * 卡片显示字段配置
 */
export interface CardFieldConfig {
  showName: boolean          // 显示姓名
  showGeneration: boolean    // 显示世代
  showBirthYear: boolean     // 显示出生年份
  showGender: boolean        // 显示性别
  showAvatar: boolean        // 显示头像
  showCustomField1?: string  // 自定义字段1（从customAttributes中提取）
  showCustomField2?: string  // 自定义字段2
}

/**
 * 布局方向配置
 */
export type LayoutDirection = 'TB' | 'BT' | 'LR' | 'RL'

/**
 * 连线样式配置
 */
export interface EdgeStyleConfig {
  fatherLineColor: string      // 父子关系线颜色
  motherLineColor: string      // 母子关系线颜色
  lineWidth: number            // 线宽
  fatherLineStyle: 'solid' | 'dashed'  // 父子关系线样式
  motherLineStyle: 'solid' | 'dashed'  // 母子关系线样式
  showShadow: boolean          // 是否显示阴影
}

/**
 * 图形显示配置
 */
export interface GraphDisplayConfig {
  layoutDirection: LayoutDirection  // 布局方向：TB=自上而下, BT=自下而上, LR=从左到右, RL=从右到左
  cardFields: CardFieldConfig       // 卡片字段配置
  nodeWidth: number                 // 节点宽度
  nodeHeight: number                // 节点高度
  nodeSpacing: number               // 节点间距
  rankSpacing: number               // 层级间距
  edgeStyle: EdgeStyleConfig        // 连线样式配置
}
